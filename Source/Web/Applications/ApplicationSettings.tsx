// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-fluentui';
import { PrimaryButton, Stack, TextField } from '@fluentui/react';
import { Remove } from 'API/applications/Remove';
import { Application as ApplicationModel } from 'API/applications/Application';
import { useNavigate, useParams } from 'react-router-dom';
import { ResourcesForApplication } from 'API/applications/ResourcesForApplication';

export interface IApplicationSettingsProps {
    application: ApplicationModel;
}

export const ApplicationSettings = (props: IApplicationSettingsProps) => {
    const removeApplicationCommand = new Remove();
    const navigate = useNavigate();

    const [resources] = ResourcesForApplication.use({ applicationId: props.application.id });

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
            <h2>Kernel</h2>
            <TextField label="Public IP Address" value={resources.data?.ipAddress || ''} readOnly disabled />

            <h2>MongoDB</h2>
            <TextField label="Server" value={resources.data?.mongoDB?.connectionString || ''} readOnly disabled />

            <h3>Users</h3>
            {resources.data?.mongoDB?.users?.map(user => {
                return (
                    <Stack key={user.userName} horizontal>
                        <TextField label="Username" value={user.userName}/>
                        <TextField label="Password" value={user.password} readOnly disabled type="password" canRevealPassword/>
                    </Stack>
                );
            })}

            <h2>Danger zone</h2>
            <PrimaryButton text="Delete" onClick={showRemoveWarning} />
        </Stack>

    );
};
