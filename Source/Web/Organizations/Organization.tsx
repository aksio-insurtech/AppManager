// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { AddSubscriptionDialog } from './AddSubscriptionDialog';
import { AddAzureSubscription } from 'API/organization/settings/AddAzureSubscription';
import { AllSettings } from 'API/organization/settings/AllSettings';
import { useEffect, useState } from 'react';
import { SetMongoDBSettings } from 'API/organization/settings/SetMongoDBSettings';
import { SetPulumiSettings } from 'API/organization/settings/SetPulumiSettings';
import { Button, Dialog, DialogActions, DialogContent, DialogContentText, DialogTitle, FormControl, FormLabel, Paper, Stack, Table, TableBody, TableCell, TableContainer, TableHead, TableRow, TextField } from '@mui/material';

export const Organization = () => {
    const [settings] = AllSettings.use();
    const [pulumiOrganization, setPulumiOrganization] = useState('');
    const [pulumiAccessToken, setPulumiAccessToken] = useState('');
    const [mongoDBOrganizationId, setMongoDBOrganizationId] = useState('');
    const [mongoDBPublicKey, setMongoDBPublicKey] = useState('');
    const [mongoDBPrivateKey, setMongoDBPrivateKey] = useState('');
    const [addSubscriptionOpen, setAddSubscriptionOpen] = useState(false);

    useEffect(() => {
        setPulumiOrganization(settings.data.pulumiOrganization);
        setPulumiAccessToken(settings.data.pulumiAccessToken);
        setMongoDBOrganizationId(settings.data.mongoDBOrganizationId);
        setMongoDBPublicKey(settings.data.mongoDBPublicKey);
        setMongoDBPrivateKey(settings.data.mongoDBPrivateKey);
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

    const openAddSubscriptionDialog = () => {
        setAddSubscriptionOpen(true);
    };

    const closeAddSubscriptionDialog = () => {
        setAddSubscriptionOpen(false);
    };

    return (
        <div style={{ margin: '1rem' }}>
            <Stack direction="column">
                <h2>Azure Subscriptions</h2>

                <TableContainer component={Paper}>
                    <Table>
                        <TableHead>
                            <TableRow>
                                <TableCell>Name</TableCell>
                                <TableCell>Tenant</TableCell>
                                <TableCell>Subscription</TableCell>
                            </TableRow>
                        </TableHead>
                        <TableBody>
                            {(settings.data?.azureSubscriptions || []).map(item => (
                                <TableRow key={item.subscriptionId}>
                                    <TableCell>{item.name}</TableCell>
                                    <TableCell>{item.tenantName}</TableCell>
                                    <TableCell>{item.subscriptionId}</TableCell>
                                </TableRow>
                            ))}
                        </TableBody>
                    </Table>
                </TableContainer>
                <Button onClick={openAddSubscriptionDialog}>Add</Button>


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

            <Dialog open={addSubscriptionOpen}>
                <DialogTitle>Add Azure Subscription</DialogTitle>
                <DialogContent>
                    <DialogContentText>
                        Hello there
                    </DialogContentText>

                    <DialogActions>
                        <Button onClick={closeAddSubscriptionDialog}>Cancel</Button>
                        <Button onClick={closeAddSubscriptionDialog}>Add</Button>
                    </DialogActions>

                </DialogContent>

            </Dialog>
        </div>
    );
};
