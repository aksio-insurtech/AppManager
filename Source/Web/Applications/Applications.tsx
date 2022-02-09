// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useState } from 'react';
import {
    Nav,
    INavLinkGroup,
    INavLink,
    INavStyles,
    CommandBar,
    ICommandBarItemProps,
    FontIcon} from '@fluentui/react';
import { Route, Routes, useNavigate, useParams } from 'react-router-dom';
import { default as styles } from './Applications.module.scss';
import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-fluentui';
import { CreateApplicationDialog } from './CreateApplicationDialog';
import { Create as CreateApplication } from 'API/applications/Create';
import { Guid } from '@aksio/cratis-fundamentals';
import { AllApplications } from 'API/applications/AllApplications';
import { Application as ApplicationModel } from 'API/applications/Application';
import { Application } from './Application';
import { CreateMicroserviceDialog } from './CreateMicroserviceDialog';
import { Create as CreateMicroservice } from 'API/applications/microservices/Create';


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
    const [selectedApplication, setSelectedApplication] = useState<ApplicationModel>();
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
    const [applications] = AllApplications.use();

    const [showCreateMicroserviceDialog] = useModal(
        'Create microservice',
        ModalButtons.OkCancel,
        CreateMicroserviceDialog,
        async (result, output) => {
            if( result == ModalResult.Success && output) {
                const command = new CreateMicroservice();
                command.applicationId = selectedApplication!.id;
                command.microserviceId = Guid.create().toString();
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
                    url: '',
                    route: `/applications/${application.id}`,
                    links: application.microservices?.map(microservice => {
                        return {
                            type: 'microservice',
                            key: microservice.microserviceId,
                            name: microservice.name,
                            url: '',
                            route: `/applications/${application.id}/${microservice.microserviceId}`,
                            links: microservice.deployables?.map(deployable => {
                                return {
                                    type: 'deployable',
                                    key: deployable.deployableId,
                                    name: deployable.name,
                                    url: '',
                                    route: `/applications/${application.id}/${microservice.microserviceId}/${deployable.deployableId}`
                                };
                            })
                        };
                    })
                };
            })
        }];

    const history = useNavigate();
    const params = useParams();
    const applicationId = params['*'] || '';

    const setSelectedApplicationFromKey = (key: string) => {
        setSelectedApplication(applications.data.find(_ => _.id == key));
    }

    const navItemClicked = (ev?: React.MouseEvent<HTMLElement>, item?: INavLink) => {
        if (item) {
            setSelectedApplicationFromKey(item.key!);
            setSelectedNav(item.key!);
            history(item.route);
        }
    };

    if (applications.data.length > 0 && !selectedApplication && applicationId !== '') {
        setSelectedApplicationFromKey(applicationId);
        setSelectedNav(applicationId);
    }

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
                            setSelectedApplicationFromKey(link.key!);
                            showCreateMicroserviceDialog();
                        }} />
                    </div>
                );
            case 'microservice':
                return (
                    <div className={styles.itemActions}>
                        <FontIcon iconName='CloudAdd' title="Start" onClick={(e) => {
                            e.preventDefault();
                            alert('hello');
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
                    {selectedApplication && <Route path=':id' element={<Application application={selectedApplication} />} />}
                </Routes>
            </div>
        </div>
    );
};
