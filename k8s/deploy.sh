#!/bin/bash

NAMESPACE="fiaptechchallenge"

echo "Criando namespace '$NAMESPACE' (se ainda não existir)..."
kubectl get namespace $NAMESPACE >/dev/null 2>&1 || kubectl create namespace $NAMESPACE

echo "Aplicando Persistent Volumes e Persistent Volume Claims..."
kubectl apply -f pvc-rabbitmq-sql.yaml

echo "Aplicando Services..."
kubectl apply -f sqlserver-service.yaml
kubectl apply -f rabbitmq-service.yaml
kubectl apply -f emailworker-service.yaml
kubectl apply -f healthscheduling-service.yaml
kubectl apply -f identity-service.yaml

echo "Aplicando deployment do SQL Server..."
kubectl apply -f sqlserver-deployment.yaml

echo "Aguardando SQL Server iniciar e aceitar conexões..."
until kubectl get pod -n $NAMESPACE -l app=sql-server -o jsonpath='{.items[0].status.phase}' | grep -q "Running"; do
  echo "SQL Server ainda não está pronto..."
  sleep 8
done

echo "Aplicando deployment do HealthScheduling e aguardando migração..."
kubectl apply -f healthscheduling-deployment.yaml
sleep 15
echo "Migração do HealthScheduling concluída!"

echo "Aplicando deployment do Identity e aguardando migração..."
kubectl apply -f identity-deployment.yaml
sleep 15
echo "Migração do Identity concluída!"

echo "Aplicando demais deployments..."
kubectl apply -f rabbitmq-deployment.yaml
kubectl apply -f emailworker-deployment.yaml

echo "Aplicando Horizontal Pod Autoscaler (HPA) para HealthScheduling..."
kubectl apply -f hpa-healthscheduling.yaml

echo "Aplicando Horizontal Pod Autoscaler (HPA) para Identity..."
kubectl apply -f hpa-identity.yaml

echo "Reiniciando todos os pods..."
kubectl rollout restart deployment -n $NAMESPACE

echo "Todos os pods foram iniciados!"
kubectl get pods -n $NAMESPACE
