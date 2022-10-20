// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ListItem, ListItemButton, ListItemIcon, ListItemText, Menu, MenuItem } from '@mui/material';
import { useState } from 'react';
import { ListItemActionButton } from './ListItemActionButton';
import * as icons from '@mui/icons-material';
import { ApplicationsHierarchyForListing } from 'API/applications/ApplicationsHierarchyForListing';
import { CreateMicroservice } from 'API/applications/environments/CreateMicroservice';
import { useNavigate } from 'react-router-dom';
import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-mui';
import { CreateMicroserviceDialog } from './CreateMicroserviceDialog';
import { Guid } from '@aksio/cratis-fundamentals';

export interface ApplicationItemProps {
    application: ApplicationsHierarchyForListing;
    actions?: boolean
}


export const ApplicationItem = (props: ApplicationItemProps) => {
    const navigate = useNavigate();
    const [environmentMenuAnchorElement, setEnvironmentMenuAnchorElement] = useState<null | HTMLElement>(null);
    const environmentOpen = Boolean(environmentMenuAnchorElement);

    const handleEnvironmentClick = (event: React.MouseEvent<HTMLElement>) => {
        setEnvironmentMenuAnchorElement(event.currentTarget);
    };

    const handleEnvironmentClose = () => {
        setEnvironmentMenuAnchorElement(null);
    };

    const [showCreateMicroservice] = useModal(
        'Create microservice',
        ModalButtons.OkCancel,
        CreateMicroserviceDialog,
        async (result, output) => {
            if (result == ModalResult.success) {
                const command = new CreateMicroservice();
                command.applicationId = output!.applicationId;
                command.microserviceId = Guid.create().toString();
                command.name = output!.name;
                await command.execute();
            }
        }
    );

    return (
        <ListItem component="div" disablePadding>
            <ListItemButton sx={{ height: 56 }} onClick={() => {
                navigate(`/applications/${props.application.id}`);
            }}>
                <ListItemIcon>
                    <icons.Home color="primary" />
                </ListItemIcon>
                <ListItemText primaryTypographyProps={{
                    color: 'primary',
                    fontWeight: 'medium',
                    variant: 'body2'
                }}>{props.application.name}</ListItemText>
            </ListItemButton>

            {props.actions &&
                <>
                    <ListItemActionButton title="Select environment" icon={<icons.AltRoute />} onClick={handleEnvironmentClick} />
                    <Menu
                        open={environmentOpen}
                        anchorEl={environmentMenuAnchorElement}
                        onClick={handleEnvironmentClose}
                        onClose={handleEnvironmentClose}
                        PaperProps={{
                            elevation: 0,
                            sx: {
                                overflow: 'visible',
                                filter: 'drop-shadow(0px 2px 8px rgba(0,0,0,0.32))',
                                mt: 1.5,
                                '& .MuiAvatar-root': {
                                    width: 32,
                                    height: 32,
                                    ml: -0.5,
                                    mr: 1,
                                },
                                '&:before': {
                                    content: '""',
                                    display: 'block',
                                    position: 'absolute',
                                    top: 0,
                                    right: 14,
                                    width: 10,
                                    height: 10,
                                    bgcolor: 'background.paper',
                                    transform: 'translateY(-50%) rotate(45deg)',
                                    zIndex: 0,
                                },
                            }
                        }}
                        transformOrigin={{ horizontal: 'right', vertical: 'top' }}
                        anchorOrigin={{ horizontal: 'right', vertical: 'bottom' }}>
                        <MenuItem>Development</MenuItem>
                        <MenuItem>Production</MenuItem>
                    </Menu>
                    <ListItemActionButton title="Add Microservice" icon={<icons.Add />} onClick={() => {
                        showCreateMicroservice({
                            applicationId: props.application.id
                        });
                    }} />
                    <ListItemActionButton title="Application Settings" icon={<icons.Settings />} arrow />
                </>
            }
        </ListItem >
    );
};
