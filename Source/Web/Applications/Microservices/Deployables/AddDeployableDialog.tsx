// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IModalProps } from '@aksio/applications-mui';
import { Checkbox, FormControlLabel, Stack, TextField } from '@mui/material';
import { useState } from 'react';

export type AddDeployableDialogOutput = {
    name: string;
    image: string | undefined;
};

export const AddDeployableDialog = (props: IModalProps<{}, AddDeployableDialogOutput>) => {
    const [name, setName] = useState('');
    const [image, setImage] = useState<string | undefined>(undefined);
    const [hasImage, setHasImage] = useState(false);

    props.onClose(() => {
        return {
            name,
            image
        };
    });

    return (
        <Stack direction="column" width={400} spacing={1}>
            <TextField label='Name' fullWidth required defaultValue={name} onChange={e => setName(e.currentTarget.value)} />
            <FormControlLabel label="Image" control={
                <Checkbox onChange={(event) => setHasImage(event.target.checked)} />} />

            {hasImage && <TextField label='Image' fullWidth required defaultValue={image} onChange={e => setImage(e.currentTarget.value)} />}
        </Stack>
    );
};
