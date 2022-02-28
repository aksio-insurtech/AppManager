// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-fluentui';
import { PrimaryButton, Stack, TextField } from '@fluentui/react';
import { Microservice as MicroserviceModel } from 'API/applications/microservices/Microservice';
import { Remove } from 'API/applications/microservices/Remove';
import { useNavigate } from 'react-router-dom';
export interface IGeneralProps {
    microservice: MicroserviceModel;
}

export const General = (props: IGeneralProps) => {
    const removeMicroserviceCommand = new Remove();
    const navigate = useNavigate();

    const [showRemoveWarning] = useModal(
        'Remove microservice?',
        ModalButtons.YesNo,
        `Are you sure you want to remove microservice '${props.microservice?.name || ''}'`,
        async (result) => {
            if (result == ModalResult.Success) {
                removeMicroserviceCommand.microserviceId = props.microservice!.id;
                await removeMicroserviceCommand.execute();
                navigate('/applications');
            }
        });

    return (
        <Stack>
            <h2>Settings</h2>
            <TextField label="Name" readOnly value={props.microservice?.name || ''} />
            <br />
            <br />
            <br />

            <h2>Danger zone</h2>
            <PrimaryButton text="Delete" onClick={showRemoveWarning} />

        </Stack>
    );
};
