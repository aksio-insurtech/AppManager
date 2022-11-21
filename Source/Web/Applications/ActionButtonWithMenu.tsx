// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { Menu } from '@mui/material';
import { useState } from 'react';
import { ListItemActionButton } from './ListItemActionButton';

export interface ActionButtonWithMenuProps {
    title: string;
    icon: JSX.Element;
    children?: JSX.Element | JSX.Element[];
}

export const ActionButtonWithMenu = (props: ActionButtonWithMenuProps) => {
    const [menuAnchorElement, setMenuAnchorElement] = useState<null | HTMLElement>(null);
    const menuOpen = Boolean(menuAnchorElement);

    const handleActionButtonClick = (event: React.MouseEvent<HTMLElement>) => {
        setMenuAnchorElement(event.currentTarget);
    };

    const handleMenuClose = () => {
        setMenuAnchorElement(null);
    };

    return (
        <>
            <ListItemActionButton title={props.title} icon={props.icon} onClick={handleActionButtonClick} />
            <Menu
                open={menuOpen}
                anchorEl={menuAnchorElement}
                onClick={handleMenuClose}
                onClose={handleMenuClose}
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
                {props.children}
            </Menu>
        </>
    );
};
