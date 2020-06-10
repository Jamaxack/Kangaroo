echo "Deleting k8s resources..."
kubectl delete deployments --all
kubectl delete services --all
kubectl delete -f ingress.yaml

echo "Deploying infrastructure components..."
kubectl create -f rabbitmq.yaml
kubectl create -f redis.yaml
kubectl create -f elasticsearch.yaml
kubectl create -f kibana.yaml
kubectl create -f mongo.yaml
kubectl create -f mssql.yaml

echo "Applying services..."
kubectl create -f services.yaml
kubectl create -f ingress.yaml

echo "Applying deployments..." 
istioctl kube-inject -f deployments.yaml | kubectl apply -f -

echo "Successfully deployed"