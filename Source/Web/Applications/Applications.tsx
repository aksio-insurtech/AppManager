// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { useState } from 'react';
import { Nav, INavLinkGroup, INavLink, INavStyles, DefaultButton } from '@fluentui/react';
import { Route, Routes, useNavigate, useParams } from 'react-router-dom';
import { default as styles } from './Applications.module.scss';
import { ModalButtons, ModalResult, useModal } from '@aksio/cratis-fluentui';
import { CreateApplicationDialog } from './CreateApplicationDialog';
import { CreateApplication } from 'API/applications/CreateApplication';
import { Guid } from '@aksio/cratis-fundamentals';
import { AllApplications } from 'API/applications/AllApplications';
import { Application as ApplicationModel } from 'API/applications/Application';
import { Application } from './Application';


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
        "Create application",
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

    const groups: INavLinkGroup[] = [
        {
            links: applications.data.map(application => {
                return {
                    key: application.id,
                    name: application.name,
                    url: '',
                    route: `/applications/${application.id}`,
                    links: application.microservices?.map(microservice => {
                        return {
                            key: microservice.microserviceId,
                            name: microservice.name,
                            url: '',
                            route: `/applications/${application.id}/${microservice.microserviceId}`,
                            links: microservice.deployables?.map(deployable => {
                                return {
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

    return (

        <div className={styles.applicationsContainer}>
            <div className={styles.applicationsNavigation}>
                <div className={styles.actions}>
                    <DefaultButton
                        text="Create"
                        title="Create application"
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
            <div>
                <Routes>
                    {selectedApplication && <Route path=':id' element={<Application application={selectedApplication} />} />}
                </Routes>
            </div>
        </div>
    );
};
