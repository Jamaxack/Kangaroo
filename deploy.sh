docker build -t jamaxack/deliveryapi -f src/Services/Delivery/Delivery.API/Dockerfile .
docker build -t jamaxack/courierapi -f src/Services/Courier/Courier.API/Dockerfile .
docker build -t jamaxack/pricingapi -f src/Services/Pricing/Pricing.API/Dockerfile .
docker build -t jamaxack/nginx ./src/ApiGateways/Nginx

# Push images to docker hub
docker push jamaxack/deliveryapi
docker push jamaxack/courierapi
docker push jamaxack/pricingapi
docker push jamaxack/nginx

kubectl apply -f src/K8s

kubectl set image deployments/delivery-deployment deliveryapi=jamaxack/deliveryapi
kubectl set image deployments/courier-deployment courierapi=jamaxack/courierapi
kubectl set image deployments/pricing-deployment pricingapi=jamaxack/pricingapi
