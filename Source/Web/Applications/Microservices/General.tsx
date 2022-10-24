// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Button, TextField } from '@mui/material';
import { Microservice } from 'API/applications/environments/microservices/Microservice';
import { Remove } from 'API/applications/microservices/Remove';
import { useNavigate } from 'react-router-dom';

export interface GeneralProps {
    microservice: Microservice;
}

export const General = (props: GeneralProps) => {
    const removeMicroserviceCommand = new Remove();
    const navigate = useNavigate();

    return (
        <div>
            <h2>Settings</h2>
            <TextField label="Name" value={props.microservice?.name || ''} inputProps={{ readOnly: true }} />
        </div>
    );
};
