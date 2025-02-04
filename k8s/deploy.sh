#!/bin/bash

NAMESPACE="fiaptechchallenge"

echo "🚀 Criando namespace '$NAMESPACE' (se ainda não existir)..."
kubectl get namespace $NAMESPACE >/dev/null 2>&1 || kubectl create namespace $NAMESPACE

echo "📦 Aplicando Persistent Volumes e Persistent Volume Claims..."
kubectl apply -f pvc-rabbitmq-sql.yaml

echo "📦 Aplicando todos os deployments no Kubernetes..."
kubectl apply -f rabbitmq-deployment.yaml
kubectl apply -f sqlserver-deployment.yaml
kubectl apply -f emailworker-deployment.yaml
kubectl apply -f healthscheduling-deployment.yaml
kubectl apply -f identity-deployment.yaml

echo "🔄 Reiniciando todos os pods para garantir que as novas imagens sejam usadas..."
kubectl rollout restart deployment -n $NAMESPACE

echo "⏳ Aguardando os pods entrarem no status 'Running'..."
while [[ $(kubectl get pods -n $NAMESPACE --no-headers | grep -c 'Running') -lt 5 ]]; do
  echo "⏳ Aguardando os pods subirem..."
  sleep 5
done

echo "✅ Todos os pods estão rodando!"
kubectl get pods -n $NAMESPACE
