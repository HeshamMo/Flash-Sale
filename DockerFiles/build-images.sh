#!/bin/bash
## run  using :  bash build-images.sh
set -e

echo "Building Gateway..."
docker build \
  -f ./Gateway.Dockerfile \
  -t flashsale-gateway \
  ../GateWay

echo "Building Order API..."
docker build \
  -f ./FlashSale.OrderManager.Dockerfile \
  -t flashsale-order \
  ../FlashSale.OrderManager.API

echo "Building Inventory API..."
docker build \
  -f ./FlashSale.InventoryManager.Dockerfile \
  -t flashsale-inventory \
  ../FlashSale.InventoryManager.API

echo ""
echo "All images built successfully!"
echo ""
docker images | grep -E "flashsale-gateway|flashsale-order|flashsale-inventory"