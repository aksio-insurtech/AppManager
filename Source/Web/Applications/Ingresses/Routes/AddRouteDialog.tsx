// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/applications-mui';
import { InputLabel, MenuItem, Select, Stack, TextField } from '@mui/material';
import { useEffect, useState } from 'react';
import { MicroservicesInEnvironment } from 'API/applications/environments/microservices/MicroservicesInEnvironment';
import { Microservice } from 'API/applications/environments/microservices/Microservice';

export type AddRouteDialogInput = {
    applicationId: string;
    environmentId: string;
    ingressId: string;
}

export type AddRouteDialogOutput = {
    path: string;
    targetPath: string;
    targetMicroservice: string;
};

export const AddRouteDialog = (props: IModalProps<AddRouteDialogInput, AddRouteDialogOutput>) => {
    const [path, setPath] = useState('/');
    const [targetPath, setTargetPath] = useState('/');
    const [selectedMicroservice, setSelectedMicroservice] = useState<Microservice | undefined>();

    const [microservices] = MicroservicesInEnvironment.use({ applicationId: props.input!.applicationId, environmentId: props.input!.environmentId });

    useEffect(() => {
        setSelectedMicroservice(microservices.data[0]);
    }, [microservices.data]);


    props.onClose(() => {
        return {
            path: path,
            targetPath: targetPath,
            targetMicroservice: selectedMicroservice!.id.microserviceId
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Path' fullWidth required defaultValue={path} onChange={e => setPath(e.currentTarget.value)} />
            <TextField label='TargetPath' fullWidth required defaultValue={targetPath} onChange={e => setTargetPath(e.currentTarget.value)} />

            {(microservices.data.length > 0 && selectedMicroservice) &&
                <>
                    <InputLabel>Microservice</InputLabel>
                    <Select
                        label="Microservice"
                        value={selectedMicroservice?.id.microserviceId}
                        fullWidth>
                        {microservices.data.map(microservice => (
                            <MenuItem key={microservice.id.microserviceId} value={selectedMicroservice?.id.microserviceId}>{microservice.name}</MenuItem>
                        ))}
                    </Select>
                </>
            }
        </Stack>
    );
};
