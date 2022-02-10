// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-fluentui';
import { PrimaryButton, Stack } from '@fluentui/react';
import { Remove } from 'API/applications/Remove';
import { Application as ApplicationModel } from 'API/applications/Application';
import { useNavigate } from 'react-router-dom';

export interface IApplicationSettingsProps {
    application?: ApplicationModel;
}

export const ApplicationSettings = (props: IApplicationSettingsProps) => {
    const removeApplicationCommand = new Remove();
    const navigate = useNavigate();

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
            <h2>Danger zone</h2>
            <PrimaryButton text="Delete" onClick={showRemoveWarning} />
        </Stack>

    );
};
