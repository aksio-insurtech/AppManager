// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Box, Tab } from '@mui/material';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { useState } from 'react';
import { Variables } from '../Variables/Variables';
import { Secrets } from '../Secrets/Secrets';
import { ConfigFiles } from '../ConfigFiles/ConfigFiles';
import { RouteParams, useRouteParams } from '../RouteParams';
import { SetEnvironmentVariableForApplicationEnvironment } from 'API/applications/environments/SetEnvironmentVariableForApplicationEnvironment';
import { EnvironmentVariablesForApplicationEnvironmentId } from 'API/applications/environments/EnvironmentVariablesForApplicationEnvironmentId';
import { SetConfigFileForApplicationEnvironment } from 'API/applications/environments/SetConfigFileForApplicationEnvironment';
import { ConfigFile } from 'API/applications/ConfigFile';
import { ConfigFilesForApplicationEnvironmentId } from 'API/applications/environments/ConfigFilesForApplicationEnvironmentId';
import { SetSecretForApplicationEnvironment } from 'API/applications/environments/SetSecretForApplicationEnvironment';
import { SecretsForApplicationEnvironmentId } from 'API/applications/environments/SecretsForApplicationEnvironmentId';
import { EnvironmentVariable } from 'API/applications/EnvironmentVariable';
import { Secret } from 'API/applications/environments/microservices/Secret';

export const Settings = () => {
    const { applicationId, environmentId, microserviceId } = useRouteParams();
    const [selectedTab, setSelectedTab] = useState("0");
    const [environmentVariablesQuery] = EnvironmentVariablesForApplicationEnvironmentId.use({ environmentId: environmentId! });
    const [configFilesQuery] = ConfigFilesForApplicationEnvironmentId.use({ environmentId: environmentId! });
    const [secretsQuery] = SecretsForApplicationEnvironmentId.use({ environmentId: environmentId! });

    const environmentVariables = environmentVariablesQuery.data?.variables ?? [];
    const configFiles = configFilesQuery.data?.files ?? [];
    const secrets = secretsQuery.data?.secrets ?? [];

    const variableSet = async (variable: EnvironmentVariable, context: RouteParams) => {
        const command = new SetEnvironmentVariableForApplicationEnvironment();
        command.applicationId = applicationId;
        command.environmentId = environmentId!;
        command.key = variable.key;
        command.value = variable.value;
        await command.execute();
    };

    const configFileSet = async (file: ConfigFile, context: RouteParams) => {
        const command = new SetConfigFileForApplicationEnvironment();
        command.applicationId = context.applicationId;
        command.environmentId = context.environmentId!;
        command.name = file.name;
        command.content = file.content;
        await command.execute();
    };

    const secretSet = async (secret: Secret, context: RouteParams) => {
        const command = new SetSecretForApplicationEnvironment();
        command.applicationId = context.applicationId;
        command.environmentId = context.environmentId!;
        command.key = secret.key;
        command.value = secret.value;
        await command.execute();
    };

    return (
        <TabContext value={selectedTab}>
            <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                <TabList onChange={(e, value) => setSelectedTab(value)}>
                    <Tab label="Config Files" value="0" />
                    <Tab label="Variables" value="1" />
                    <Tab label="Secrets" value="2" />
                </TabList>
            </Box>
            <TabPanel value="0"><ConfigFiles onConfigFileSet={configFileSet} files={configFiles} /></TabPanel>
            <TabPanel value="1"><Variables onVariableSet={variableSet} variables={environmentVariables} /></TabPanel>
            <TabPanel value="2"><Secrets onSecretSet={secretSet} secrets={secrets} /></TabPanel>
        </TabContext>
    );
};
