// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useEffect, useState } from 'react';
import {
    Nav,
    INavLinkGroup,
    INavLink,
    INavStyles,
    CommandBar,
    ICommandBarItemProps,
    FontIcon
} from '@fluentui/react';
import { Route, Routes, useNavigate, useParams, useLocation, matchPath } from 'react-router-dom';
import { default as styles } from './Applications.module.scss';
import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-fluentui';
import { CreateApplicationDialog } from './CreateApplicationDialog';
import { Create as CreateApplication } from 'API/applications/Create';
import { Guid } from '@aksio/cratis-fundamentals';
import { ApplicationsHierarchy } from 'API/applications/ApplicationsHierarchy';
import { Application } from './Application';
import { CreateMicroserviceDialog } from './CreateMicroserviceDialog';
import { Create as CreateMicroservice } from 'API/applications/microservices/Create';
import { Create as CreateDeployable } from 'API/applications/microservices/deployables/Create';
import { CreateDeployableDialog } from './CreateDeployableDialog';
import { Microservice } from './Microservices/Microservice';


const navStyles: Partial<INavStyles> = {
    root: {
        width: 200
    },
    link: {
        whiteSpace: 'normal',
        lineHeight: 'inherit',
    },
};

export const Applications = () => {
    const [selectedNav, setSelectedNav] = useState('');
    const [currentApplication, setCurrentApplication] = useState<string>();
    const [currentMicroservice, setCurrentMicroservice] = useState<string>();
    const [currentDeployable, setCurrentDeployable] = useState<string>();
    const navigate = useNavigate();
    const location = useLocation();

    const routes: string[] = [
        '/applications/:applicationId',
        '/applications/:applicationId/microservices/:microserviceId',
        '/applications/:applicationId/microservices/:microserviceId/deployables/:deployableId'
    ];

    useEffect(() => {
        const match = routes.map(_ => matchPath({ path: _ }, location.pathname)).filter(_ => _ !== null);
        if (match?.length == 1) {
            const params = match[0]?.params || {};

            setCurrentApplication(params.applicationId);
            setCurrentMicroservice(params.microserviceId);
            setCurrentDeployable(params.deployableId);

            setSelectedNav(params.deployableId || params.microserviceId || params.applicationId || '');
        }
    }, [location.pathname]);

    const [showCreateApplicationDialog] = useModal(
        'Create application',
        ModalButtons.OkCancel,
        CreateApplicationDialog,
        async (result, output) => {
            if (result == ModalResult.Success && output) {
                const command = new CreateApplication();
                command.applicationId = Guid.create().toString();
                command.name = output.name;
                command.azureSubscriptionId = output.azureSubscription;
                command.cloudLocation = output.cloudLocation;
                await command.execute();
            }
        }
    );
    const [applications] = ApplicationsHierarchy.use();

    const [showCreateMicroserviceDialog] = useModal(
        'Create microservice',
        ModalButtons.OkCancel,
        CreateMicroserviceDialog,
        async (result, output) => {
            if (result == ModalResult.Success && output) {
                const command = new CreateMicroservice();
                command.applicationId = currentApplication!;
                command.microserviceId = Guid.create().toString();
                command.name = output.name;
                await command.execute();
            }
        }
    );

    const [showCreateDeployableDialog] = useModal(
        'Create deployable',
        ModalButtons.OkCancel,
        CreateDeployableDialog,
        async (result, output) => {
            if (result == ModalResult.Success && output) {
                const command = new CreateDeployable();
                command.microserviceId = currentMicroservice!;
                command.deployableId = Guid.create().toString();
                command.name = output.name;
                await command.execute();
            }
        }
    );

    const groups: INavLinkGroup[] = [
        {
            links: applications.data.map(application => {
                return {
                    type: 'application',
                    key: application.id,
                    name: application.name,
                    isExpanded: true,
                    url: '',
                    route: `/applications/${application.id}`,
                    links: application.microservices?.map(microservice => {
                        return {
                            type: 'microservice',
                            key: microservice.microserviceId,
                            name: microservice.name,
                            isExpanded: true,
                            url: '',
                            route: `/applications/${application.id}/microservices/${microservice.microserviceId}`,
                            links: microservice.deployables?.map(deployable => {
                                return {
                                    type: 'deployable',
                                    key: deployable.deployableId,
                                    name: deployable.name,
                                    isExpanded: true,
                                    url: '',
                                    route: `/applications/${application.id}/microservices/${microservice.microserviceId}/deployables/${deployable.deployableId}`
                                };
                            })
                        };
                    })
                };
            })
        }];


    const navItemClicked = (ev?: React.MouseEvent<HTMLElement>, item?: INavLink) => {
        if (item) {
            switch ((item as any).type) {
                case 'application':
                    setCurrentApplication(item.key!);
                    break;

                case 'microservice':
                    setCurrentMicroservice(item.key!);
                    break;

                case 'deployable':
                    setCurrentDeployable(item.key!);
                    break;
            }

            setSelectedNav(item.key!);
            navigate(item.route);
        }
    };

    const commandBarItems: ICommandBarItemProps[] = [
        {
            key: 'createApplication',
            name: 'Application',
            title: 'Create application',
            iconProps: { iconName: 'Add' },
            onClick: showCreateApplicationDialog
        }
    ];

    const actions = (link: INavLink, type: string): JSX.Element => {
        switch (type) {
            case 'application':
                return (
                    <div className={styles.itemActions}>
                        <FontIcon iconName='WebAppBuilderFragmentCreate' title="Start" onClick={(e) => {
                            e.preventDefault();
                            setCurrentApplication(link.key!);
                            showCreateMicroserviceDialog();
                        }} />
                    </div>
                );
            case 'microservice':
                return (
                    <div className={styles.itemActions}>
                        <FontIcon iconName='CloudAdd' title="Start" onClick={(e) => {
                            e.preventDefault();
                            setCurrentMicroservice(link.key!);
                            showCreateDeployableDialog();
                        }} />
                    </div>
                );
        }

        return (
            <></>
        );
    };

    const renderLink = (link: INavLink): JSX.Element => {
        return (
            <>
                <div>
                    {link.name}
                </div>
                {actions(link, (link as any).type)}
            </>
        );
    };

    return (

        <div className={styles.applicationsContainer}>
            <div className={styles.applicationsNavigation}>
                <div className={styles.actions}>
                    <CommandBar items={commandBarItems} />
                </div>
                <Nav
                    groups={groups}
                    styles={navStyles}
                    onLinkClick={navItemClicked}
                    onRenderLink={renderLink as any}
                    selectedKey={selectedNav} />
            </div>
            <div>
                <Routes>
                    <Route path=':applicationId' element={<Application />} />
                    <Route path=':applicationId/microservices/:microserviceId' element={<Microservice />} />
                </Routes>
            </div>
        </div>
    );
};
