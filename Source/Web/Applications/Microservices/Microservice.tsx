// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GetMicroservice } from 'API/applications/environments/microservices/GetMicroservice';
import { Box, Tab } from '@mui/material';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { useState } from 'react';
import { Variables } from '../Variables/Variables';
import { Secrets } from '../Secrets/Secrets';
import { Deployables } from './Deployables/Deployables';
import { ConfigFiles } from '../ConfigFiles/ConfigFiles';
import { General } from './General';
import { RouteParams, useRouteParams } from '../RouteParams';
import { SetEnvironmentVariableForMicroservice } from 'API/applications/environments/microservices/SetEnvironmentVariableForMicroservice';
import { EnvironmentVariablesForMicroserviceId } from 'API/applications/environments/microservices/EnvironmentVariablesForMicroserviceId';
import { ConfigFile } from 'API/applications/ConfigFile';
import { SetConfigFileForMicroservice } from 'API/applications/environments/microservices/SetConfigFileForMicroservice';
import { ConfigFilesForMicroserviceId } from 'API/applications/environments/microservices/ConfigFilesForMicroserviceId';
import { SetSecretForMicroservice } from 'API/applications/environments/microservices/SetSecretForMicroservice';
import { EnvironmentVariable } from 'API/applications/EnvironmentVariable';
import { Secret } from 'API/applications/environments/microservices/Secret';
import { SecretsForMicroserviceId } from 'API/applications/environments/microservices/SecretsForMicroserviceId';

export const Microservice = () => {
    const { applicationId, environmentId, microserviceId } = useRouteParams();
    const [microservice, performMicroserviceQuery] = GetMicroservice.use({
        applicationId: applicationId!,
        environmentId: environmentId!,
        microserviceId: microserviceId!
    });
    const [selectedTab, setSelectedTab] = useState("0");
    const [environmentVariablesQuery] = EnvironmentVariablesForMicroserviceId.use({ microserviceId: microserviceId! });
    const [configFilesQuery] = ConfigFilesForMicroserviceId.use({ microserviceId: microserviceId! });
    const [secretsQuery] = SecretsForMicroserviceId.use({ microserviceId: microserviceId! });

    const environmentVariables = environmentVariablesQuery.data?.variables ?? [];
    const configFiles = configFilesQuery.data?.files ?? [];
    const secrets = secretsQuery.data?.secrets ?? [];

    const variableSet = async (variable: EnvironmentVariable, context: RouteParams) => {
        const command = new SetEnvironmentVariableForMicroservice();
        command.applicationId = applicationId;
        command.environmentId = environmentId!;
        command.microserviceId = context.microserviceId!;
        command.key = variable.key;
        command.value = variable.value;
        await command.execute();
    };

    const configFileSet = async (file: ConfigFile, context: RouteParams) => {
        const command = new SetConfigFileForMicroservice();
        command.applicationId = context.applicationId;
        command.environmentId = context.environmentId!;
        command.microserviceId = context.microserviceId!;
        command.name = file.name;
        command.content = file.content;
        await command.execute();
    };

    const secretSet = async (secret: Secret, context: RouteParams) => {
        const command = new SetSecretForMicroservice();
        command.applicationId = context.applicationId;
        command.environmentId = context.environmentId!;
        command.microserviceId = context.microserviceId!;
        command.key = secret.key;
        command.value = secret.value;
        await command.execute();
    };

    return (
        <TabContext value={selectedTab}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <TabList onChange={(e, value) => setSelectedTab(value)}>
                    <Tab label="General" value="0" />
                    <Tab label="Deployables" value="1" />
                    <Tab label="Config Files" value="2" />
                    <Tab label="Variables" value="3" />
                    <Tab label="Secrets" value="4" />
                </TabList>
            </Box>
            <TabPanel value="0"><General microservice={microservice.data} /></TabPanel>
            <TabPanel value="1"><Deployables /></TabPanel>
            <TabPanel value="2"><ConfigFiles onConfigFileSet={configFileSet} files={configFiles} /></TabPanel>
            <TabPanel value="3"><Variables onVariableSet={variableSet} variables={environmentVariables}/></TabPanel>
            <TabPanel value="4"><Secrets onSecretSet={secretSet} secrets={secrets}/></TabPanel>
        </TabContext>
    );
};
