// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { AddSubscriptionDialog } from './AddSubscriptionDialog';
import { AddAzureSubscription } from 'API/settings/AddAzureSubscription';
import { AllSettings } from 'API/settings/AllSettings';
import { useEffect, useState } from 'react';
import { AzureSubscription } from 'API/settings/AzureSubscription';
import { SetMongoDBSettings } from 'API/settings/SetMongoDBSettings';
import { SetPulumiSettings } from 'API/settings/SetPulumiSettings';
import { Box, Button, Stack, TextField } from '@mui/material';
import { DataGrid, GridColDef } from '@mui/x-data-grid';
import { useModal, ModalButtons, ModalResult } from '@aksio/cratis-mui';
import { SetAzureServicePrincipal } from 'API/settings/SetAzureServicePrincipal';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 },
    { field: 'tenantName', headerName: 'Tenant', width: 250 },
    { field: 'subscriptionId', headerName: 'Subscription ID', width: 250 }
];

export const Settings = () => {
    const [settings] = AllSettings.use();
    const [pulumiOrganization, setPulumiOrganization] = useState('');
    const [pulumiAccessToken, setPulumiAccessToken] = useState('');
    const [mongoDBOrganizationId, setMongoDBOrganizationId] = useState('');
    const [mongoDBPublicKey, setMongoDBPublicKey] = useState('');
    const [mongoDBPrivateKey, setMongoDBPrivateKey] = useState('');
    const [clientId, setClientId] = useState('');
    const [clientSecret, setClientSecret] = useState('');

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
        setClientId(settings.data.servicePrincipal?.clientId || '');
        setClientSecret(settings.data.servicePrincipal?.clientSecret || '');
        setPulumiOrganization(settings.data.pulumiOrganization || '');
        setPulumiAccessToken(settings.data.pulumiAccessToken || '');
        setMongoDBOrganizationId(settings.data.mongoDBOrganizationId || '');
        setMongoDBPublicKey(settings.data.mongoDBPublicKey || '');
        setMongoDBPrivateKey(settings.data.mongoDBPrivateKey || '');
    }, [settings.data]);

    const saveAzureServicePrincipal = async () => {
        const command = new SetAzureServicePrincipal();
        command.clientId = clientId;
        command.clientSecret = clientSecret;
        await command.execute();
    }

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

                <h2>Azure Service Principal</h2>
                <TextField label='Service Principal Id (ClientId)' fullWidth required value={clientId} onChange={(e) => setClientId(e.currentTarget.value)} />
                <TextField label='Service Principal Secret (ClientSecret)' type="password" fullWidth required value={clientSecret} onChange={(e) => setClientSecret(e.currentTarget.value)} />
                <Button onClick={saveAzureServicePrincipal}>Save</Button>

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
