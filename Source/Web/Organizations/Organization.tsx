// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { AddSubscriptionDialog } from './AddSubscriptionDialog';
import { AddAzureSubscription } from 'API/organization/settings/AddAzureSubscription';
import { AllSettings } from 'API/organization/settings/AllSettings';
import { AzureSubscription } from 'API/organization/settings/AzureSubscription';
import { useEffect, useState } from 'react';
import { SetMongoDBSettings } from 'API/organization/settings/SetMongoDBSettings';
import { SetPulumiSettings } from 'API/organization/settings/SetPulumiSettings';
import { Box, Button, Stack, TextField } from '@mui/material';
import { DataGrid, GridColDef, GridValueGetterParams } from '@mui/x-data-grid';
import { useModal, ModalButtons, ModalResult } from '@aksio/cratis-mui';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 },
    { field: 'tenantName', headerName: 'Tenant', width: 250 },
    { field: 'subscriptionId', headerName: 'Subscription ID', width: 250 }
];

export const Organization = () => {
    const [settings] = AllSettings.use();
    const [pulumiOrganization, setPulumiOrganization] = useState('');
    const [pulumiAccessToken, setPulumiAccessToken] = useState('');
    const [mongoDBOrganizationId, setMongoDBOrganizationId] = useState('');
    const [mongoDBPublicKey, setMongoDBPublicKey] = useState('');
    const [mongoDBPrivateKey, setMongoDBPrivateKey] = useState('');

    const [showAddSubscription] = useModal(
        'Add Azure subscription',
        ModalButtons.OkCancel,
        AddSubscriptionDialog,
        async (result, output) => {
            if (result == ModalResult.success) {
                const command = new AddAzureSubscription();
                command.id = output!.id;
                command.name = output!.name;
                command.tenantId = output!.tenantId;
                command.tenantName = output!.tenantName;
                await command.execute();
            }
        }
    );

    useEffect(() => {
        setPulumiOrganization(settings.data.pulumiOrganization || '');
        setPulumiAccessToken(settings.data.pulumiAccessToken || '');
        setMongoDBOrganizationId(settings.data.mongoDBOrganizationId || '');
        setMongoDBPublicKey(settings.data.mongoDBPublicKey || '');
        setMongoDBPrivateKey(settings.data.mongoDBPrivateKey || '');
    }, [settings.data]);

    const savePulumiSettings = async () => {
        const command = new SetPulumiSettings();
        command.organization = pulumiOrganization;
        command.accessToken = pulumiAccessToken;
        await command.execute();
    };

    const saveMongoDBSettings = async () => {
        const command = new SetMongoDBSettings();
        command.organizationId = mongoDBOrganizationId;
        command.publicKey = mongoDBPublicKey;
        command.privateKey = mongoDBPrivateKey;
        await command.execute();
    };

    return (
        <div style={{ margin: '1rem' }}>
            <Stack direction="column">
                <h2>Azure Subscriptions</h2>
                <Box sx={{ height: 400, width: '100%' }}>
                    <DataGrid
                        hideFooterPagination={true}
                        filterMode="client"
                        columns={columns}
                        sortingMode="client"
                        getRowId={(row: AzureSubscription) => {
                            return row.subscriptionId;
                        }}
                        rows={settings.data?.azureSubscriptions || []} />
                </Box>
                <Button onClick={showAddSubscription}>Add</Button>

                <h2>Pulumi</h2>
                <TextField label="Organization" value={pulumiOrganization} onChange={(e) => setPulumiOrganization(e.currentTarget.value)} />
                <TextField label="Access Token" type="password" value={pulumiAccessToken} onChange={e => setPulumiAccessToken(e.currentTarget.value)} />
                <Button onClick={savePulumiSettings}>Save</Button>

                <h2>MongoDB Atlas</h2>
                <TextField label="OrganizationId" value={mongoDBOrganizationId} onChange={e => setMongoDBOrganizationId(e.currentTarget.value)} />
                <TextField label="Public Key" value={mongoDBPublicKey} onChange={e => setMongoDBPublicKey(e.currentTarget.value)} />
                <TextField label="Private Key" value={mongoDBPrivateKey} type="password" onChange={e => setMongoDBPrivateKey(e.currentTarget.value)} />
                <Button onClick={saveMongoDBSettings}>Save</Button>
            </Stack>
        </div>
    );
};
