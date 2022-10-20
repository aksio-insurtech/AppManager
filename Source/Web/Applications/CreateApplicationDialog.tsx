// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React, { useState } from 'react';
import { AllCloudLocations } from 'API/cloudlocations/AllCloudLocations';
import { AllSettings } from 'API/settings/AllSettings';
import { MenuItem, Select, TextField, Stack, FormControl, InputLabel } from '@mui/material';
import { IModalProps } from '@aksio/cratis-mui';

export interface CreateApplicationDialogOutput {
    name: string;
    azureSubscription: string;
    cloudLocation: string;
}

export const CreateApplicationDialog = (props: IModalProps<{}, CreateApplicationDialogOutput>) => {
    const [name, setName] = useState('');
    const [azureSubscription, setAzureSubscription] = useState('');
    const [settings] = AllSettings.use();
    const [cloudLocation, setCloudLocation] = useState('');
    const [cloudLocations] = AllCloudLocations.use();

    props.onClose(() => {
        return {
            name,
            azureSubscription,
            cloudLocation
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />

            <InputLabel>Azure Subscription</InputLabel>
            <Select
                label="Azure Subscription"
                placeholder="Select the Azure Subscription"
                value={azureSubscription}
                onChange={(ev) => {
                    setAzureSubscription(ev.target.value);
                }}>
                {settings.data.azureSubscriptions?.map(_ => (
                    <MenuItem key={_.subscriptionId} value={_.subscriptionId}>{_.name}</MenuItem>
                ))}
            </Select>

            <InputLabel>Location</InputLabel>
            <Select
                label="Location"
                placeholder="Select the location in the cloud"
                value={cloudLocation}
                onChange={(ev) => {
                    setCloudLocation(ev.target.value);
                }}>
                {cloudLocations.data.map(_ => (
                    <MenuItem key={_.key} value={_.key}>{_.displayName}</MenuItem>
                ))}
            </Select>
        </Stack>
    );
};
