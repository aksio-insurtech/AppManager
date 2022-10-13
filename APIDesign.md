#

## Settings

### Pulumi

Route: /api/settings/pulumi
Method: POST
Purpose: Set pulumi organization and access token

### MongoDB

Route: /api/settings/mongodb
Method: POST
Purpose: Set MongoDB public & private keys

### Azure Subscriptions

Route: /api/settings/azure/subscriptions/{environment}
Method: POST
Purpose: Set subscription and associated service principal for a specific environment (Development, Production)

## Applications

### Application

Route: /api/applications
Method: POST
Purpose: Create application

### Remove application

Route: /api/applications/{applicationId}/remove
Method: POST
Purpose: Remove application

### Ingresses

Route: /api/applications/{applicationId}/{environment}/ingresses
Method: POST
Purpose: Add ingress to application

### Authentication

Route: /api/applications/{applicationId}/{environment}/ingresses/{ingressId}/authentication
Method: POST
Purpose: Configure authentication for application

## Microservices

### Microservice

Route: /api/applications/{applicationId}/{environment}/microservices
Method: POST
Purpose: Create microservice

### Remove microservice

Route: /api/applications/{applicationId}/{environment}/microservices/{microserviceId}/remove
Method: POST
Purpose: Remove Microservice

## Deployables

### Deployable

Route: /api/applications/{applicationId}/{environment}/microservices/{microserviceId}/deployables
Method: POST
Purpose: Create deployable

### Deployable image

Route: /api/applications/{applicationId}/{environment}/microservices/{microserviceId}/deployables/{deployableId}/image
Method: POST
Purpose: Change deployable image (include SHA and version)
