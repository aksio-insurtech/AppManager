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



