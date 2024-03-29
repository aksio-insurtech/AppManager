# [v0.17.1] - 2023-7-25 [PR: #184](https://github.com/aksio-insurtech/AppManager/pull/184)

### Fixed

- Fixing the fully qualified name for the custom Serilog formatter we use. This has moved into a different repository.


# [v0.16.0] - 2023-6-14 [PR: #182](https://github.com/aksio-insurtech/AppManager/pull/182)

### Added

- Adding support for impersonation.


# [v0.14.0] - 2023-5-23 [PR: #180](https://github.com/aksio-insurtech/AppManager/pull/180)

### Added

- Adds support for relative paths for certificates when using JSON files with the Bootstrapper. This assumes relative path based on the variables file.


# [v0.12.0] - 2023-5-22 [PR: #179](https://github.com/aksio-insurtech/AppManager/pull/179)

## Summary

- This PR enforces more security in some of the resources the appmanager sets up.

### Fixes

Fixed a bug ingress-middleware template, OAuthBearerTokens was not camelcase and didn't work.

### Security

- Key vaults now only accepts traffic from the application environment vnet.
- Storage accounts now only accepts traffic from the application environment vnet, with the option to add external ip-adresses to storage account accesslist. 
  This is used like this in the configuration json:
```json
"environment": {
    (...)
    "storage": {
        "accessList": [
            { 
                "name": "Main office",
                "address": "1.2.3.4"
            },
            { 
                "name": "Secondary office", 
                "address": "2.4.6.8"
            }
        ]
    }
}
```

- Changed nginx logging to include the "x-fowarded-for" header, as it holds the actual originating IP of the request.
- Rejects any ingress definitions that do not have any identity solution defined (identity provider, oauth bearertoken or accesslist).
- Extended ingress configuration options to work with a regexp-style ingress path. 
  - This requires a dns resolver to be set as well on the ingress, and a flag on the route hinting that it should be used.
  - This allows us to basically accesslist paths even with variable components, having a 404 fallback.
  - Here is an example ingress definition that accepts calls to "/swagger" and "/###/prefixedroute/#####", which then requires the use of useResolver (or nginx will fail):
```json
{
    "id": "06519d43-7f1f-4f99-8f5a-3b4162dd913c",
    "name": "someapp-ingress",
    "resolver": "8.8.8.8",
    "domain": {
        "name": "someapp.qa.mydomain.com",
        "certificateId": "83ce7060-5e48-48b6-b9b6-f72ce96c768a"
    },
    "routes": [
        {
            "path": "/swagger",
            "targetMicroservice": "bc31e022-fcbc-4341-a2dc-a8ad49fd6c12",
            "targetPath": "/swagger"
        },
        {
            "path": "~ '^/([\\d]{3})/prefixedroute/([\\d]{5})'",
            "targetMicroservice": "bc31e022-fcbc-4341-a2dc-a8ad49fd6c12",
            "targetPath": "/$1/prefixedroute/$2",
            "useResolver": true
        }
    ]
}
```

# [v0.11.1] - 2023-4-13 [PR: #175](https://github.com/aksio-insurtech/AppManager/pull/175)

### Fixed

- Cratis upgraded to 8.13.2


# [v0.10.0] - 2023-4-13 [PR: #174](https://github.com/aksio-insurtech/AppManager/pull/174)

### Added

- Made it possible to target the Cratis Kernel as a route

### Changed

- Setting default JSON formatter for logging to Serilogs compact formatter.
- Upgraded Cratis



# [v0.8.0] - 2023-4-3 [PR: #173](https://github.com/aksio-insurtech/AppManager/pull/173)

### Added

- Made Cratis optional and is now a resource that can be added
- Made MongoDB optional and is now a resource that can be added
- Made Azure Container registry optional and is now a resource that can be added

### Changed

- Changed `ConfigPath` to be `MountPath`

### Fixed

- Azure Files mounted storage for container apps was read only, made them writable.
- Setting HTTP logging to default information
- Explicitly setting Pulumi Organization to use
- Removing unwanted `Console.WriteLine()` in service bus renderer
- Adding JSON access log output for Nginx Ingresses
- Removing the `WriteTo` section for Serilog in `AppSettings.json` so that we get to override it.


# [v0.7.1] - 2023-2-27 [PR: #135](https://github.com/aksio-insurtech/AppManager/pull/135)

### Fixed

- Removed temporary endpoint for associating application environment with application. This was a temporary need to get an event in.


# [v0.6.0] - 2023-2-27 [PR: #134](https://github.com/aksio-insurtech/AppManager/pull/134)

### Added

- Application level endpoint for changing image on a deployable


# [v0.5.8] - 2023-2-24 [PR: #133](https://github.com/aksio-insurtech/AppManager/pull/133)

## Summary

Testing out the continuous deployment feature.

# [v0.5.6] - 2023-2-24 [PR: #122](https://github.com/aksio-insurtech/AppManager/pull/122)

### Added

- Shared resource group for resources shared across environments, such as registry.
- View of deployments, status and log output
- Supporting setting image on a deployable

### Fixed

- Environment variables are now possible to add, edit and view across all levels.
- Secrets are now possible to add, edit and view across all levels.
- Config files are now possible to add, edit and view across all levels.
- Adding tags to application insight component
- Fixing templates
- Fixed configuration of Cratis after its breaking changes.



# [v0.5.4] - 2022-12-28 [PR: #120](https://github.com/aksio-insurtech/AppManager/pull/120)

### Fixed

- Fixing path for GitHub action that builds the diagnostics image.


# [v0.5.1] - 2022-12-28 [PR: #119](https://github.com/aksio-insurtech/AppManager/pull/119)

### Fixed

- Using Azure private endpoints connection string for MongoDB.
- Adding null checks for domains.


# [v0.4.0] - 2022-12-27 [PR: #118](https://github.com/aksio-insurtech/AppManager/pull/118)

### Added

- Config path for deployables is now overridable
- Introducing resource system
- Implemented Service Bus as resource
- Outbox -> Inbox configuration in place
- VNET configuration done
- Azure AD issuer made configurable, we can now chose multi/single tenant
- Custom domains up and running


# [v0.3.41] - 2022-11-27 [PR: #107](https://github.com/aksio-insurtech/AppManager/pull/107)

### Fixed

- Configuring Nginx correctly for WebSockets by moving upgrade and all needed headers into the location for each route.
- Upgraded Cratis with improved WebSockets support.


# [v0.3.39] - 2022-11-26 [PR: #106](https://github.com/aksio-insurtech/AppManager/pull/106)

### Fixed

- Basically end to end working with identity & custom domains.


# [v0.3.35] - 2022-9-15 [PR: #91](https://github.com/aksio-insurtech/AppManager/pull/91)

### Fixed

- Consistently using Microservice if it is a microservice update.


# [v0.3.33] - 2022-9-15 [PR: #89](https://github.com/aksio-insurtech/AppManager/pull/89)

### Fixed

- Import the Microservice and not the Application when updating a microservice.


# [v0.3.29] - 2022-9-15 [PR: #87](https://github.com/aksio-insurtech/AppManager/pull/87)

### Fixed

- Reverting on getting existing fileshare before creating. Doesn't really work.


# [v0.3.27] - 2022-9-15 [PR: #86](https://github.com/aksio-insurtech/AppManager/pull/86)

### Fixed

- Getting existing fileshare for Microservice, if not exists create it


# [v0.3.25] - 2022-9-15 [PR: #85](https://github.com/aksio-insurtech/AppManager/pull/85)

### Fixed

- Making all Pulumi operations asynchronous.


# [v0.3.23] - 2022-9-14 [PR: #83](https://github.com/aksio-insurtech/AppManager/pull/83)

### Fixed

- Fixing so that we're consistent on what a stack is linked to. We now have a "top level" stack per application and one stack per microservice. And all of these are per environment as well.


# [v0.3.21] - 2022-9-13 [PR: #80](https://github.com/aksio-insurtech/AppManager/pull/80)

### Fixed

- Adding a consolidation endpoint for making sure we get events out.


# [v0.3.19] - 2022-9-12 [PR: #79](https://github.com/aksio-insurtech/AppManager/pull/79)

### Fixed

- Getting existing resource group for application for microservice stacks, instead of creating new ones - which will conflict.


# [v0.3.17] - 2022-9-12 [PR: #78](https://github.com/aksio-insurtech/AppManager/pull/78)

### Fixed

- Build errors.


# [v0.3.15] - 2022-9-12 [PR: #76](https://github.com/aksio-insurtech/AppManager/pull/76)

### Fixed

- Stacks object coming in on controller is a Json document - fixed so that we do `.ToString()` on it for the `StackDeployment` to be handed as a string.


# [v0.3.11] - 2022-9-5 [PR: #72](https://github.com/aksio-insurtech/AppManager/pull/72)

### Fixed

- Adding logging messages for `Stacks` saving and getting.


# [v0.3.9] - 2022-9-4 [PR: #71](https://github.com/aksio-insurtech/AppManager/pull/71)

### Fixed

- Bootstrapper should now be able to deploy the AppManager and then hand over the stack it created to the cloud counterpart engine.


# [v0.3.7] - 2022-9-3 [PR: #69](https://github.com/aksio-insurtech/AppManager/pull/69)

### Fixed

- Persisting stack per application and importing it for every operation.


# [v0.3.5] - 2022-9-3 [PR: #68](https://github.com/aksio-insurtech/AppManager/pull/68)

### Fixed

- Wrapping all definitions in `PulumiFn.Create()`. We changed this behavior to be able to do the bootstrap, while forgetting to update the reactions.


# [v0.3.1] - 2022-9-2 [PR: #66](https://github.com/aksio-insurtech/AppManager/pull/66)

### Fixed

- For debugging purposes; adding logging of application content and settings content.


# [v0.2.0] - 2022-9-2 [PR: #65](https://github.com/aksio-insurtech/AppManager/pull/65)

### Added

- Azure Service Principal is now possible to configure with the API


# [v0.1.21] - 2022-9-2 [PR: #64](https://github.com/aksio-insurtech/AppManager/pull/64)



# [v0.1.19] - 2022-9-2 [PR: #63](https://github.com/aksio-insurtech/AppManager/pull/63)

### Fixed

- Fixing cache keys used for sharing data between jobs so that we are actually building new things


# [v0.1.17] - 2022-9-2 [PR: #62](https://github.com/aksio-insurtech/AppManager/pull/62)

### Fixed

- Additional logging for Pulumi operations


# [v0.1.15] - 2022-9-2 [PR: #61](https://github.com/aksio-insurtech/AppManager/pull/61)

### Fixed

- Moving configuration of environment variables for Pulumi into the program.


# [v0.1.12] - 2022-9-2 [PR: #59](https://github.com/aksio-insurtech/AppManager/pull/59)

### Fixed

- Upgrading to the latest Cratis. `appsettings.json` should now also be loaded from the `config` folder.


# [v0.1.11] - 2022-9-1 [PR: #58](https://github.com/aksio-insurtech/AppManager/pull/58)

### Fixed

- Moving the setting of the Pulumi access token to an earlier stage.


# [v0.1.10] - 2022-9-1 [PR: #57](https://github.com/aksio-insurtech/AppManager/pull/57)



# [v0.1.9] - 2022-9-1 [PR: #56](https://github.com/aksio-insurtech/AppManager/pull/56)



# [v0.1.8] - 2022-9-1 [PR: #52](https://github.com/aksio-insurtech/AppManager/pull/52)

### Fixed

- 🤞🏻 Maybe we have the ENV $PATH properly set for Pulumi tooling to work.


# [v0.1.7] - 2022-9-1 [PR: #51](https://github.com/aksio-insurtech/AppManager/pull/51)

### Fixed

- Hopefully we get Pulumi in the path with this release.


# [v0.1.6] - 2022-9-1 [PR: #50](https://github.com/aksio-insurtech/AppManager/pull/50)

### Fixed

- Fixing so that we actually export the path to Pulumi, not just setting PATH.


# [v0.1.5] - 2022-9-1 [PR: #49](https://github.com/aksio-insurtech/AppManager/pull/49)

### Fixed

- Adding the path to Pulumi to the PATH environment variable


# [v0.1.4] - 2022-9-1 [PR: #48](https://github.com/aksio-insurtech/AppManager/pull/48)

### Fixed

- Adding error message to the log if failing during Pulumi Operations.


# [v0.1.3] - 2022-9-1 [PR: #47](https://github.com/aksio-insurtech/AppManager/pull/47)

### Fixed

- Added Deployable immediate projection definition.


# [v0.1.2] - 2022-8-31 [PR: #46](https://github.com/aksio-insurtech/AppManager/pull/46)



# [v0.1.1] - 2022-8-31 [PR: #44](https://github.com/aksio-insurtech/AppManager/pull/44)



# [v0.1.0] - 2022-8-31 [PR: #43](https://github.com/aksio-insurtech/AppManager/pull/43)



# [v0.0.3] - 2022-8-30 [PR: #41](https://github.com/aksio-insurtech/AppManager/pull/41)



