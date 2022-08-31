// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import React, { useState } from 'react';
import { AllCloudLocations } from 'API/cloudlocations/AllCloudLocations';
import { AllSettings } from 'API/organization/settings/AllSettings';
import { MenuItem, Select, TextField } from '@mui/material';

export interface CreateApplicationDialogOutput {
    name: string;
    azureSubscription: string;
    cloudLocation: string;
}

export const CreateApplicationDialog = (props: CreateApplicationDialogOutput) => {
    const [name, setName] = useState('');
    const [azureSubscription, setAzureSubscription] = useState('');
    const [settings] = AllSettings.use();
    const [cloudLocation, setCloudLocation] = useState('');
    const [cloudLocations] = AllCloudLocations.use();

    return (
        <div>
            <TextField label='Name' required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />
            {/* <Select placeholder="Select the Azure Subscription"
                label="Azure Subscription">
                {azureSubscriptionOptions.map(_ => (
                    <MenuItem key={_.key}>{_.name}</MenuItem>
                ))}
            </Select>

            <Select placeholder="Select a location in the cloud"
                label="Azure Subscription">
                {cloudLocationOptions.map(_ => (
                    <MenuItem key={_.key}>{_.name}</MenuItem>
                ))}
            </Select> */}
        </div>
    );
};
