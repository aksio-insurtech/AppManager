// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { INavLinkGroup, INavStyles, Nav, Stack, TextField } from '@fluentui/react';

const groups: INavLinkGroup[] = [
    {
        links: [
            {
                name: 'General',
                url: '#',
                icon: 'Settings'
            }
        ]
    },
    {
        name: 'Configuration',
        links: [
            {
                name: 'Environment variables',
                url: '#',
                icon: 'Variable'
            },
            {
                name: 'Configuration files',
                url: '#',
                icon: 'ConfigurationSolid'
            }

        ]
    },
    {
        name: '3rd parties',
        links: [
        ]
    }
];

const navStyles: Partial<INavStyles> = {
    root: {
        width: 200
    },
    link: {
        whiteSpace: 'normal',
        lineHeight: 'inherit',
    },
};

function onRenderGroupHeader(group: INavLinkGroup): JSX.Element {
    return <small>{group.name}</small>;
}


export const MicroserviceSettings = () => {


    return (
        <Stack horizontal>
            <Nav
                onRenderGroupHeader={onRenderGroupHeader as any}
                groups={groups}
                styles={navStyles}
                />

            <Stack.Item>

                <Stack>
                    <TextField label="Name"/>
                </Stack>

            </Stack.Item>

        </Stack>
    );
};
