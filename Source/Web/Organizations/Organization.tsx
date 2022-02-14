// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-fluentui';
import { CommandBar, DetailsList, IColumn, Stack, ICommandBarItemProps, TextField, PrimaryButton } from '@fluentui/react';
import { AddSubscriptionDialog } from './AddSubscriptionDialog';
import { AddAzureSubscription } from 'API/organization/settings/AddAzureSubscription';
import { AllSettings } from 'API/organization/settings/AllSettings';
import { useEffect, useState } from 'react';
import { SetPulumiAccessToken } from '../API/organization/settings/SetPulumiAccessToken';
import { SetMongoDBSettings } from '../API/organization/settings/SetMongoDBSettings';

const subscriptionsColumns: IColumn[] = [
    {
        key: 'name',
        name: 'Name',
        fieldName: 'name',
        minWidth: 200
    },
    {
        key: 'id',
        name: 'Id',
        fieldName: 'subscriptionId',
        minWidth: 250
    }
];

export const Organization = () => {
    const [settings] = AllSettings.use();
    const [pulumiAccessToken, setPulumiAccessToken] = useState('');
    const [mongoDBOrganizationId, setMongoDBOrganizationId] = useState('');
    const [mongoDBPublicKey, setMongoDBPublicKey] = useState('');
    const [mongoDBPrivateKey, setMongoDBPrivateKey] = useState('');

    const [showAddSubscription] = useModal(
        'Add subscription',
        ModalButtons.OkCancel,
        AddSubscriptionDialog,
        async (result, output) => {
            if (result == ModalResult.Success && output) {
                const command = new AddAzureSubscription();
                command.id = output.id;
                command.name = output.name;
                await command.execute();
            }
        });

    const subscriptionsCommandBarItems: ICommandBarItemProps[] = [
        {
            key: 'add',
            text: 'Add subscription',
            iconProps: { iconName: 'Add' },
            onClick: showAddSubscription
        }
    ];

    useEffect(() => {
        setPulumiAccessToken(settings.data.pulumiAccessToken);
        setMongoDBOrganizationId(settings.data.mongoDBOrganizationId);
        setMongoDBPublicKey(settings.data.mongoDBPublicKey);
        setMongoDBPrivateKey(settings.data.mongoDBPrivateKey);
    }, [settings.data]);

    const savePulumiAccessToken = async () => {
        const command = new SetPulumiAccessToken();
        command.accessToken = pulumiAccessToken;
        await command.execute();
    };

    const saveMongoDBKeys = async () => {
        const command = new SetMongoDBSettings();
        command.organizationId = mongoDBOrganizationId;
        command.publicKey = mongoDBPublicKey;
        command.privateKey = mongoDBPrivateKey;
        await command.execute();
    };

    return (
        <div style={{ margin: '1rem' }}>
            <Stack>
                <Stack.Item>
                    <h2>Azure Subscriptions</h2>
                    <CommandBar items={subscriptionsCommandBarItems} />
                    <DetailsList columns={subscriptionsColumns} items={settings.data?.azureSubscriptions || []} />
                </Stack.Item>

                <Stack.Item>
                    <h2>Pulumi</h2>
                    <TextField label="Access Token" value={pulumiAccessToken} onChange={(_, value) => setPulumiAccessToken(value!)} />
                    <PrimaryButton text='Save' onClick={savePulumiAccessToken} />
                </Stack.Item>

                <Stack.Item>
                    <h2>MongoDB Atlas</h2>
                    <TextField label="OrganizationId" value={mongoDBOrganizationId} onChange={(_, value) => setMongoDBOrganizationId(value!)} />
                    <TextField label="Public Key" value={mongoDBPublicKey} onChange={(_, value) => setMongoDBPublicKey(value!)} />
                    <TextField label="Private Key" value={mongoDBPrivateKey} onChange={(_, value) => setMongoDBPrivateKey(value!)} />
                    <PrimaryButton text='Save' onClick={saveMongoDBKeys} />
                </Stack.Item>
            </Stack>
        </div>
    );
};
