// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useState } from 'react';
import { Nav, INavLinkGroup, INavLink, INavStyles, DefaultButton } from '@fluentui/react';
import { useNavigate } from 'react-router-dom';

import { default as styles } from './Applications.module.scss';
import { IModalProps, ModalButtons, ModalResult, useModal } from '@aksio/cratis-fluentui';
import { CreateApplicationDialog } from './CreateApplicationDialog';


const navStyles: Partial<INavStyles> = {
    root: {
        width: 200
    },
    link: {
        whiteSpace: 'normal',
        lineHeight: 'inherit',
    },
};

const groups: INavLinkGroup[] = [
    {
        links: [
            {
                name: 'Home',
                url: '',
                route: '/'
            },
            {
                name: 'Applications',
                url: 'applications',
                route: '/applications'
            }
        ]
    }
];


export const Applications = () => {
    const [selectedNav, setSelectedNav] = useState('');
    const [showCreateApplicationDialog] = useModal(
        "Create application",
        ModalButtons.OkCancel,
        CreateApplicationDialog,
        (result, output) => {
            if (result == ModalResult.Success && output) {
            }
        }
    );

    const history = useNavigate();

    const navItemClicked = (ev?: React.MouseEvent<HTMLElement>, item?: INavLink) => {
    };
    return (

        <div className={styles.applicationsContainer}>
            <div className={styles.applicationsNavigation}>
                <div className={styles.actions}>
                    <DefaultButton
                        text="Create"
                        style={{ width: 135 }}
                        iconProps={{ iconName: "Add" }}
                        onClick={showCreateApplicationDialog} />
                </div>
                <Nav
                    groups={groups}
                    styles={navStyles}
                    onLinkClick={navItemClicked}
                    selectedKey={selectedNav} />
            </div>
            <div style={{ width: '100%' }}>
                Hello
            </div>
        </div>
    );
};
