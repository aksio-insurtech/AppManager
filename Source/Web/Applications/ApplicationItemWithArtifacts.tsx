// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Box, Divider, ListItemButton, ListItemIcon, ListItemText } from '@mui/material';
import { ApplicationsHierarchyForListing } from 'API/applications/ApplicationsHierarchyForListing';
import { ApplicationItem } from './ApplicationItem';
import * as icons from '@mui/icons-material';

export interface ApplicationItemWithArtifactsProps {
    application: ApplicationsHierarchyForListing;
}

export const ApplicationItemWithArtifacts = (props: ApplicationItemWithArtifactsProps) => {
    return (
        <>
            <ApplicationItem application={props.application} actions />
            <Divider />
            <Box
                sx={{
                    bgcolor: 'rgba(71, 98, 130, 0.2)',
                    pb: 2,
                }}>

                <ListItemButton sx={{
                    px: 3,
                    pt: 2.5,
                    pb: 0,
                    '&:hover, &focus': { '& svg': { opacity: 1 } }
                }}>
                    <ListItemIcon><icons.Input /></ListItemIcon>
                    <ListItemText>Ingress</ListItemText>
                </ListItemButton>
            </Box>

            {props.application.microservices?.map(microservice => {
                return (
                    <Box key={microservice.name}
                        sx={{
                            bgcolor: 'rgba(71, 98, 130, 0.2)',
                            pb: 2,
                        }}>

                        <ListItemButton sx={{
                            px: 3,
                            pt: 2.5,
                            pb: 0,
                            '&:hover, &focus': { '& svg': { opacity: 1 } }
                        }}>
                            <ListItemIcon><icons.Cabin /></ListItemIcon>
                            <ListItemText>Members</ListItemText>
                        </ListItemButton>
                    </Box>
                );
            })}

        </>
    );
};
