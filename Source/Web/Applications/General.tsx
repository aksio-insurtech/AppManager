// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-fluentui';
import { Link, PrimaryButton, Stack, TextField } from '@fluentui/react';
import { Remove } from 'API/applications/Remove';
import { Application as ApplicationModel } from 'API/applications/Application';
import { useNavigate } from 'react-router-dom';
import { ResourcesForApplication } from 'API/applications/ResourcesForApplication';
import { AllSettings } from 'API/organization/settings/AllSettings';

export interface IGeneralProps {
    application: ApplicationModel;
}

export const General = (props: IGeneralProps) => {
    const removeApplicationCommand = new Remove();
    const navigate = useNavigate();
    const [settings] = AllSettings.use();
    const [resources] = ResourcesForApplication.use({ applicationId: props.application.id });

    const subscription = settings.data?.azureSubscriptions?.find(_ => _.subscriptionId === resources.data?.azure?.subscriptionId);

    const [showRemoveWarning] = useModal(
        'Remove application?',
        ModalButtons.YesNo,
        `Are you sure you want to remove application '${props.application!.name}'`,
        async (result) => {
            if (result == ModalResult.Success) {
                removeApplicationCommand.applicationId = props.application!.id;
                await removeApplicationCommand.execute();
                navigate('/applications');
            }
        });

    return (
        <Stack>
            <h2>Settings</h2>
            <TextField label="Name" readOnly value={props.application.name}/>
            <br />

            <h2>Azure</h2>
            <Link target='_blank' href={`https://portal.azure.com/#@${subscription?.tenantName}/resource${resources.data?.azure?.resourceGroupId}/overview`}>Resource Group</Link>
            <Link target='_blank' href={`https://portal.azure.com/#@${subscription?.tenantName}/resource${resources.data?.azure?.resourceGroupId}/providers/Microsoft.Storage/storageAccounts/${resources.data?.azure?.storageAccountName}/overview`}>Storage Account</Link>
            <br />

            <h3>Azure Container Registry</h3>
            <TextField label="Login Server" value={resources.data?.azure?.containerRegistryLoginServer || ''} readOnly disabled />
            <TextField label="UserName" value={resources.data?.azure?.containerRegistryUserName || ''} readOnly disabled />
            <TextField label="Password" value={resources.data?.azure?.containerRegistryPassword || ''} readOnly disabled type="password" canRevealPassword/>
            <br />

            <h2>MongoDB</h2>
            <TextField label="Server" value={resources.data?.mongoDB?.connectionString || ''} readOnly disabled />

            <h3>Users</h3>
            {resources.data?.mongoDB?.users?.map(user => {
                return (
                    <Stack key={user.userName} horizontal>
                        <TextField label="Username" value={user.userName} />
                        <TextField label="Password" value={user.password} readOnly disabled type="password" canRevealPassword />
                    </Stack>
                );
            })}
            <br />
            <br />
            <br />

            <h2>Danger zone</h2>
            <PrimaryButton text="Delete" onClick={showRemoveWarning} />
        </Stack>

    );
};
