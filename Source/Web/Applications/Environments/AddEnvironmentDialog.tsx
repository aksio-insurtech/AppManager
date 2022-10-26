// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-mui';
import { InputLabel, MenuItem, Select, Stack, TextField } from '@mui/material';
import { useState } from 'react';
import { AllCloudLocations } from 'API/cloudlocations/AllCloudLocations';
import { AllSettings } from 'API/settings/AllSettings';


export type AddEnvironmentDialogOutput = {
    name: string;
    displayName: string;
    shortName: string;
    azureSubscription: string;
    cloudLocation: string;
};

export const AddEnvironmentDialog = (props: IModalProps<{}, AddEnvironmentDialogOutput>) => {
    const [name, setName] = useState('');
    const [displayName, setDisplayName] = useState('');
    const [shortName, setShortName] = useState('');
    const [azureSubscription, setAzureSubscription] = useState('');
    const [settings] = AllSettings.use();
    const [cloudLocation, setCloudLocation] = useState('');
    const [cloudLocations] = AllCloudLocations.use();

    props.onClose(() => {
        return {
            name,
            displayName,
            shortName,
            azureSubscription,
            cloudLocation
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />
            <TextField label='Display Name' fullWidth required defaultValue={displayName} onChange={e => setDisplayName(e.currentTarget.value)} />
            <TextField label='Short Name' fullWidth required defaultValue={shortName} onChange={e => setShortName(e.currentTarget.value)} />

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
