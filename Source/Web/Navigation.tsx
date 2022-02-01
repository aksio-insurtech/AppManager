// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useState } from 'react';
import { Nav, INavLinkGroup, INavLink, INavStyles } from '@fluentui/react';
import { useLocation, useNavigate } from 'react-router-dom';

import { default as styles } from './Navigation.module.scss';

const navStyles: Partial<INavStyles> = {
    root: {
        width: 158
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
                key: '',
                name: 'Home',
                url: '',
                route: '/'
            },
            {
                key: 'organization',
                name: 'Organization',
                url: 'organization',
                route: '/organization'
            },
            {
                key: 'applications',
                name: 'Applications',
                url: 'applications',
                route: '/applications'
            }
        ]
    }
];

const getSelectedNav = () => {
    const segments = document.location.pathname.split('/').filter(_ => _.length > 0);
    if( segments.length > 0 )
    {
        return segments[0];
    }

    return '';
};

export const Navigation = () => {
    const [selectedNav, setSelectedNav] = useState(getSelectedNav());
    const navigate = useNavigate();

    const navItemClicked = (ev?: React.MouseEvent<HTMLElement>, item?: INavLink) => {
        if (item) {
            ev?.preventDefault();
            setSelectedNav(item.key!);
            navigate(item.route);
        }
    };

    return (
        <div className={styles.navigationContainer}>
            <Nav
                groups={groups}
                styles={navStyles}
                onLinkClick={navItemClicked}
                selectedKey={selectedNav} />
        </div>
    );
};
