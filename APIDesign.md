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

### Authentication

Route: /api/applications/{applicationId}/authentication
Method: POST
Purpose: Configure authentication for application

### Remove application

Route: /api/applications/{applicationId}/remove
Method: POST
Purpose: Remove application

## Microservices

### Microservice

Route: /api/applications/{applicationId}/microservices
Method: POST
Purpose: Create microservice

### Remove microservice

Route: /api/applications/{applicationId}/microservices/{microserviceId}/remove
Method: POST
Purpose: Remove Microservice

## Deployables

### Deployable

Route: /api/applications/{applicationId}/microservices/{microserviceId}/deployables
Method: POST
Purpose: Create deployable

### Deployable image

Route: /api/applications/{applicationId}/microservices/{microserviceId}/deployables/{deployableId}/image
Method: POST
Purpose: Change deployable image (include SHA and version)
