// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-fluentui';
import { Dropdown, IDropdownOption, Stack, TextField } from '@fluentui/react';
import React, { useState } from 'react';
import { AllCloudLocations } from 'API/cloudlocations/AllCloudLocations';
import { AllSettings } from 'API/organization/settings/AllSettings';

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

    props.onClose(result => {
        return {
            name,
            azureSubscription,
            cloudLocation
        };
    });

    const cloudLocationOptions: IDropdownOption[] = cloudLocations.data.map(_ => {
        return {
            key: _.key,
            text: _.displayName
        };
    });

    if (cloudLocations.data.length > 0 && cloudLocation == '') {
        setCloudLocation(cloudLocations.data[0].key);
    }

    const azureSubscriptionOptions: IDropdownOption[] = settings.data?.azureSubscriptions?.map(_ => {
        return {
            key: _.subscriptionId,
            text: _.name
        };
    });

    return (
        <div>
            <Stack>
                <TextField label='Name' required defaultValue={name} onChange={(e, n) => setName(n!)} />
                <Dropdown placeholder="Select the Azure Subscription"
                    label="Azure Subscription"
                    options={azureSubscriptionOptions}
                    selectedKey={azureSubscription}
                    onChange={(_, item) => setAzureSubscription(item?.key as string || '')} />

                <Dropdown placeholder="Select a location in the cloud"
                    label="Cloud location"
                    options={cloudLocationOptions}
                    selectedKey={cloudLocation}
                    onChange={(_, item) => setCloudLocation(item?.key as string || '')} />
            </Stack>
        </div>
    );
};
