// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Box, ListItemButton, ListItemIcon, ListItemText } from '@mui/material';
import { MouseEventHandler } from 'react';

export interface ApplicationArtifactListItemProps {
    icon: JSX.Element;
    title: string,
    onClick?: MouseEventHandler<HTMLDivElement> | undefined;
}

export const ApplicationArtifactListItem = (props: ApplicationArtifactListItemProps) => {
    return (
        <Box
            sx={{
                bgcolor: 'rgba(71, 98, 130, 0.2)',
                pb: 2,
            }}>

            <ListItemButton
                sx={{
                    px: 3,
                    pt: 2.5,
                    pb: 0,
                    '&:hover, &focus': { '& svg': { opacity: 1 } }
                }}
                onClick={props.onClick}>
                <ListItemIcon>{props.icon}</ListItemIcon>
                <ListItemText>{props.title}</ListItemText>
            </ListItemButton>
        </Box>
    );
};
