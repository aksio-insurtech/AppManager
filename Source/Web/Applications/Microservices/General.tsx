// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Button, TextField } from '@mui/material';
import { Microservice as MicroserviceModel } from 'API/applications/environments/microservices/Microservice';
import { Remove } from 'API/applications/microservices/Remove';
import { useNavigate } from 'react-router-dom';
export interface IGeneralProps {
    microservice: MicroserviceModel;
}

export const General = (props: IGeneralProps) => {
    const removeMicroserviceCommand = new Remove();
    const navigate = useNavigate();

    return (
        <div>
            <h2>Settings</h2>
            <TextField label="Name" value={props.microservice?.name || ''} inputProps={{ readOnly: true }} />
            <br />
            <br />
            <br />

            <h2>Danger zone</h2>
            <Button variant="outlined">Delete</Button>
        </div>
    );
};
