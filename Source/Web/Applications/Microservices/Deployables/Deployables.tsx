// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { ValueEditorFor } from 'Components';
import { AddDeployableDialog, AddDeployableDialogOutput } from './AddDeployableDialog';
import { Box, Stack } from '@mui/system';
import { TabContext, TabList, TabPanel } from '@mui/lab';
import { Tab } from '@mui/material';
import { useState } from 'react';
import { Secrets } from '../../Secrets/Secrets';
import { Variables } from '../../Variables/Variables';
import { CreateDeployable } from 'API/applications/environments/microservices/deployables/CreateDeployable';
import { CreateDeployableWithImage } from 'API/applications/environments/microservices/deployables/CreateDeployableWithImage';
import { Guid } from '@aksio/cratis-fundamentals';
import { DeployablesForMicroservice } from 'API/applications/environments/microservices/deployables/DeployablesForMicroservice';
import { Deployable } from 'API/applications/environments/microservices/deployables/Deployable';
import { RouteParams, useRouteParams } from '../../RouteParams';
import { SetEnvironmentVariableForDeployable } from 'API/applications/environments/microservices/deployables/SetEnvironmentVariableForDeployable';
import { EnvironmentVariablesForDeployableId } from 'API/applications/environments/microservices/deployables/EnvironmentVariablesForDeployableId';
import { SetSecretForDeployable } from 'API/applications/environments/microservices/deployables/SetSecretForDeployable';
import { Secret } from 'API/applications/Secret';
import { SecretsForDeployableId } from 'API/applications/environments/microservices/deployables/SecretsForDeployableId';
import { EnvironmentVariable } from 'API/applications/EnvironmentVariable';
import { ConfigFiles } from '../../ConfigFiles/ConfigFiles';
import { SetConfigFileForDeployable } from 'API/applications/environments/microservices/deployables/SetConfigFileForDeployable';
import { ConfigFile } from 'API/applications/ConfigFile';
import { ConfigFilesForDeployableId } from 'API/applications/environments/microservices/deployables/ConfigFilesForDeployableId';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 },
    { field: 'image', headerName: 'Image', width: 250 }
];

export const Deployables = () => {
    const { applicationId, environmentId, microserviceId } = useRouteParams();
    const [selectedTab, setSelectedTab] = useState("0");
    const [deployables] = DeployablesForMicroservice.use({
        applicationId: applicationId!,
        environmentId: environmentId!,
        microserviceId: microserviceId!
    });

    const [selectedDeployable, setSelectedDeployable] = useState<Deployable | undefined>(undefined);
    const [environmentVariablesQuery] = EnvironmentVariablesForDeployableId.use({
        applicationId: applicationId!,
        environmentId: environmentId!,
        microserviceId: microserviceId!,
        deployableId: selectedDeployable?.id.deployableId ?? undefined!
    });
    const [configFilesQuery] = ConfigFilesForDeployableId.use({
        applicationId: applicationId!,
        environmentId: environmentId!,
        microserviceId: microserviceId!,
        deployableId: selectedDeployable?.id.deployableId ?? undefined!
    });
    const [secretsQuery] = SecretsForDeployableId.use({
        applicationId: applicationId!,
        environmentId: environmentId!,
        microserviceId: microserviceId!,
        deployableId: selectedDeployable?.id.deployableId ?? undefined!
    });

    const configFiles = configFilesQuery.data?.files ?? [];
    const environmentVariables = environmentVariablesQuery.data?.variables ?? [];
    const secrets = secretsQuery.data?.secrets ?? [];

    const configFileSet = async (file: ConfigFile, context: RouteParams) => {
        const command = new SetConfigFileForDeployable();
        command.applicationId = context.applicationId;
        command.environmentId = context.environmentId!;
        command.microserviceId = context.microserviceId!;
        command.deployableId = selectedDeployable!.id.deployableId;
        command.name = file.name;
        command.content = file.content;
        await command.execute();
    };

    const variableSet = async (variable: EnvironmentVariable, context: RouteParams) => {
        const command = new SetEnvironmentVariableForDeployable();
        command.applicationId = context.applicationId;
        command.environmentId = context.environmentId!;
        command.microserviceId = context.microserviceId!;
        command.deployableId = selectedDeployable!.id.deployableId;
        command.key = variable.key;
        command.value = variable.value;
        await command.execute();
    };

    const secretSet = async (secret: Secret, context: RouteParams) => {
        const command = new SetSecretForDeployable();
        command.applicationId = context.applicationId;
        command.environmentId = context.environmentId!;
        command.microserviceId = context.microserviceId!;
        command.deployableId = selectedDeployable!.id.deployableId;
        command.key = secret.key;
        command.value = secret.value;
        await command.execute();
    };

    return (
        <Stack
            direction="column"
            justifyContent="flex-start"
            spacing={5}>

            <ValueEditorFor<AddDeployableDialogOutput, Deployable>
                addTitle="Add deployable"
                columns={columns}
                data={deployables.data}
                modalContent={AddDeployableDialog}
                getRowId={(deployable) => deployable.id.deployableId}
                onSelectionChanged={deployables => {
                    if (deployables.length > 0) {
                        setSelectedDeployable(deployables[0]);
                    }
                }}
                modalClosed={async (result, output) => {
                    if (result == ModalResult.success) {
                        if (output!.image) {
                            const command = new CreateDeployableWithImage();
                            command.applicationId = applicationId!;
                            command.environmentId = environmentId!;
                            command.microserviceId = microserviceId!;
                            command.deployableId = Guid.create().toString();
                            command.name = output!.name;
                            command.image = output!.image;
                            await command.execute();
                        } else {
                            const command = new CreateDeployable();
                            command.applicationId = applicationId!;
                            command.environmentId = environmentId!;
                            command.microserviceId = microserviceId!;
                            command.deployableId = Guid.create().toString();
                            command.name = output!.name;
                            await command.execute();
                        }
                    }
                }} />

            {selectedDeployable &&
                <TabContext value={selectedTab}>
                    <Box sx={{ borderBottom: 1, borderColor: 'divider' }}>
                        <TabList onChange={(e, value) => setSelectedTab(value)}>
                            <Tab label="General" value="0" />
                            <Tab label="Config Files" value="1" />
                            <Tab label="Variables" value="2" />
                            <Tab label="Secrets" value="3" />
                        </TabList>
                    </Box>
                    <TabPanel value="0"></TabPanel>
                    <TabPanel value="1"><ConfigFiles onConfigFileSet={configFileSet} files={configFiles} /></TabPanel>
                    <TabPanel value="2"><Variables onVariableSet={variableSet} variables={environmentVariables} /></TabPanel>
                    <TabPanel value="3"><Secrets onSecretSet={secretSet} secrets={secrets} /></TabPanel>
                </TabContext>
            }
        </Stack>
    );
};
