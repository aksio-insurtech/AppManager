// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { IconButton, SxProps, Theme, Tooltip } from '@mui/material';
import * as icons from '@mui/icons-material';
import { MouseEventHandler } from 'react';

export interface ListItemActionButtonProps {
    title: string,
    icon: JSX.Element,
    arrow?: boolean
    onClick?: MouseEventHandler<HTMLButtonElement> | undefined;
}

export const ListItemActionButton = (props: ListItemActionButtonProps) => {
    const animation: SxProps<Theme> = props.arrow ? {
        '& svg': {
            color: 'rgba(255,255,255,0.8)',
            transition: '0.2s',
            transform: 'translateX(0) rotate(0)',
        },
        '&:hover, &:focus': {
            bgcolor: 'unset',
            '& svg:first-of-type': {
                transform: 'translateX(-4px) rotate(-20deg)',
            },
            '& svg:last-of-type': {
                right: 0,
                opacity: 1,
            },
        }
    } : {};

    const style: SxProps<Theme> = {
        ...{
            '&:after': {
                content: '""',
                position: 'absolute',
                height: '80%',
                display: 'block',
                left: 0,
                width: '1px',
                bgcolor: 'divider',
            },
        },
        ...animation
    };

    return (
        <Tooltip title={props.title}>
            <IconButton
                onClick={props.onClick}
                size="large"
                sx={style}>
                {props.icon}
                {props.arrow &&
                    <icons.ArrowRight sx={{ position: 'absolute', right: 4, opacity: 0 }} />
                }
            </IconButton>
        </Tooltip>
    );
};
