#!/bin/bash

# Definir o namespace
NAMESPACE="fiaptechchallenge"

echo "🚀 Criando namespace '$NAMESPACE' (se ainda não existir)..."
kubectl get namespace $NAMESPACE >/dev/null 2>&1 || kubectl create namespace $NAMESPACE

echo "📦 Aplicando todos os manifests no Kubernetes..."
kubectl apply -k . -n $NAMESPACE

echo "🔄 Reiniciando todos os pods para garantir que as novas imagens sejam usadas..."
kubectl rollout restart deployment -n $NAMESPACE

echo "⏳ Aguardando os pods entrarem no status 'Running'..."
kubectl wait --for=condition=Ready pod --all -n $NAMESPACE --timeout=120s

echo "✅ Todos os pods no namespace '$NAMESPACE' estão prontos!"
kubectl get pods -n $NAMESPACE
