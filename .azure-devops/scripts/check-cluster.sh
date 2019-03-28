#!/bin/bash

set -o nounset
set -o errexit

# This snippet is from https://blog.gripdev.xyz/2018/10/19/kubernetes-integration-testing-minikube-azure-pipelines-happy/

echo; echo "Waiting for cluster to be usable..."
JSONPATH='{range .items[*]}{@.metadata.name}:{range @.status.conditions[*]}{@.type}={@.status};{end}{end}'
until kubectl get nodes -o jsonpath="$JSONPATH" 2>&1 | grep -q "Ready=True"
    do sleep 1
done

JSONPATH='{range .items[*]}{@.metadata.name}:{range @.status.conditions[*]}{@.type}={@.status};{end}{end}'
until kubectl -n kube-system get pods -lcomponent=kube-addon-manager -o jsonpath="$JSONPATH" 2>&1 | grep -q "Ready=True"
    do sleep 1
    echo "waiting for kube-addon-manager to be available"
    kubectl get pods --all-namespaces
done

JSONPATH='{range .items[*]}{@.metadata.name}:{range @.status.conditions[*]}{@.type}={@.status};{end}{end}'
until kubectl -n kube-system get pods -lk8s-app=kube-dns -o jsonpath="$JSONPATH" 2>&1 | grep -q "Ready=True"
    do sleep 1
    echo "waiting for kube-dns to be available"
    kubectl get pods --all-namespace
done
