#!/bin/bash

NAMESPACE="fiaptechchallenge"

echo "🚀 Criando namespace '$NAMESPACE' (se ainda não existir)..."
kubectl get namespace $NAMESPACE >/dev/null 2>&1 || kubectl create namespace $NAMESPACE

echo "📡 Aplicando Persistent Volumes e Services..."
kubectl apply -f pvc-sql.yaml
kubectl apply -f sqlserver-service.yaml
kubectl apply -f healthscheduling-service.yaml
kubectl apply -f identity-service.yaml

echo "📦 Aplicando deployment do SQL Server..."
kubectl apply -f sqlserver-deployment.yaml

echo "⏳ Aguardando SQL Server iniciar e aceitar conexões..."
until kubectl get pod -n $NAMESPACE -l app=sql-server -o jsonpath='{.items[0].status.phase}' | grep -q "Running"; do
  echo "⏳ SQL Server ainda não está pronto..."
  sleep 8
done

echo "📦 Aplicando deployment do HealthScheduling..."
kubectl apply -f healthscheduling-deployment.yaml
echo "Aguardando migração do HealthScheduling..."
sleep 15
echo "✅ Migração do HealthScheduling concluída!"

echo "📦 Aplicando deployment do Identity..."
kubectl apply -f identity-deployment.yaml
echo "Aguardando migração do Identity..."
sleep 15
echo "✅ Migração do Identity concluída!"

echo "📊 Aplicando Horizontal Pod Autoscaler (HPA)..."
kubectl apply -f hpa-healthscheduling.yaml
kubectl apply -f hpa-identity.yaml
echo "✅ HPAs aplicados!"

echo "🚀 Deploy concluído com sucesso!"
