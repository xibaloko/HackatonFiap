#!/bin/bash

NAMESPACE="fiaptechchallenge"

echo "🚨 Resetando o ambiente Kubernetes no namespace '$NAMESPACE'..."

# 1️⃣ Remover todos os deployments, pods e serviços
echo "🗑️ Removendo todos os deployments, pods e serviços..."
kubectl delete all --all -n $NAMESPACE --ignore-not-found

# 2️⃣ Remover os Persistent Volume Claims (PVCs)
echo "🗑️ Removendo Persistent Volume Claims..."
kubectl delete pvc --all -n $NAMESPACE --ignore-not-found

# 3️⃣ Aguardar alguns segundos para garantir que os PVCs foram excluídos
sleep 5

# 4️⃣ Remover os Persistent Volumes (PVs)
echo "🗑️ Removendo Persistent Volumes..."
kubectl delete pv --all --ignore-not-found

# 5️⃣ Remover o namespace
echo "🗑️ Removendo namespace '$NAMESPACE'..."
kubectl delete namespace $NAMESPACE --ignore-not-found
kubectl wait --for=delete namespace/$NAMESPACE --timeout=30s 2>/dev/null

# 6️⃣ Remover manualmente os dados armazenados nos volumes do host (se estiver rodando localmente)
echo "🧹 Limpando diretórios de volumes no host..."
sudo rm -rf /mnt/data/sqlserver
sudo rm -rf /mnt/data/rabbitmq