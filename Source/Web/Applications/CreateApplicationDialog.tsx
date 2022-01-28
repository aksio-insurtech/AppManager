// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/cratis-fluentui';
import { Dropdown, IDropdownOption, Stack, TextField } from '@fluentui/react';
import React, { useState } from 'react';
import { AllCloudLocations } from 'API/Applications/AllCloudLocations';

export interface CreateApplicationDialogOutput {
    name: string;
    cloudLocation: string;
}

export const CreateApplicationDialog = (props: IModalProps<{}, CreateApplicationDialogOutput>) => {
    const [name, setName] = useState('');
    const [cloudLocation, setCloudLocation] = useState('');
    const [cloudLocations] = AllCloudLocations.use();

    props.onClose(result => {
        return {
            name,
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

    const onCloudLocationChange = (event: React.FormEvent<HTMLDivElement>, item?: IDropdownOption) => {
        setCloudLocation(item?.key as string || '');
    };

    return (
        <div>
            <Stack>
                <TextField label='Name' required defaultValue={name} onChange={(e, n) => setName(n!)} />
                <Dropdown placeholder="Select a location in the cloud"
                    label="Cloud location"
                    options={cloudLocationOptions}
                    selectedKey={cloudLocation}
                    onChange={onCloudLocationChange} />
            </Stack>
        </div>
    );
};
