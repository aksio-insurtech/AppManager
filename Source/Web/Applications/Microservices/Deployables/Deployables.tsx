// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { useParams } from 'react-router-dom';
import { Tenant } from 'API/applications/environments/tenants/Tenant';
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

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 },
    { field: 'image', headerName: 'Image', width: 250 }
];

export const Deployables = () => {
    const { applicationId, environmentId, microserviceId } = useParams();
    const [selectedTab, setSelectedTab] = useState("0");
    const [deployables] = DeployablesForMicroservice.use({
        applicationId: applicationId!,
        environmentId: environmentId!,
        microserviceId: microserviceId!
    });

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
                <TabPanel value="1"></TabPanel>
                <TabPanel value="2"><Variables /></TabPanel>
                <TabPanel value="3"><Secrets /></TabPanel>
            </TabContext>
        </Stack>
    );
};
