// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ApplicationHierarchyForListing } from 'API/applications/ApplicationHierarchyForListing';
import { useState } from 'react';
import { ListItemActionButton } from './ListItemActionButton';
import * as icons from '@mui/icons-material';
import { Menu, MenuItem } from '@mui/material';

export interface ApplicationEnvironmentButtonProps {
    application: ApplicationHierarchyForListing
}

export const ApplicationEnvironmentButton = (props: ApplicationEnvironmentButtonProps) => {
    const [environmentMenuAnchorElement, setEnvironmentMenuAnchorElement] = useState<null | HTMLElement>(null);
    const environmentOpen = Boolean(environmentMenuAnchorElement);

    const handleEnvironmentClick = (event: React.MouseEvent<HTMLElement>) => {
        setEnvironmentMenuAnchorElement(event.currentTarget);
    };

    const handleEnvironmentClose = () => {
        setEnvironmentMenuAnchorElement(null);
    };

    return (
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
                {props.application.environments.map(environment => {
                    return (
                        <MenuItem key={environment.environmentId}>{environment.name}</MenuItem>
                    );
                })}
            </Menu>
        </>
    );
};

