// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { useParams } from 'react-router-dom';
import { ValueEditorFor } from 'Components';
import { AddCustomDomainDialog, AddCustomDomainDialogOutput } from './AddCustomDomainDialog';
import { AddCustomDomainToApplicationEnvironment } from 'API/applications/environments/AddCustomDomainToApplicationEnvironment';
import { CustomDomainConfigurationForApplicationEnvironment } from 'API/applications/environments/CustomDomainConfigurationForApplicationEnvironment';
import { CustomDomain } from 'API/applications/environments/CustomDomain';

const columns: GridColDef[] = [
    { field: 'domain', headerName: 'Name', width: 250 }
];

export const CustomDomains = () => {
    const { applicationId, environmentId } = useParams();
    const [domainsForEnvironment] = CustomDomainConfigurationForApplicationEnvironment.use({ applicationId: applicationId!, environmentId: environmentId! });
    const domains = domainsForEnvironment.data.domains ?? [];

    return (
        <ValueEditorFor<AddCustomDomainDialogOutput, CustomDomain>
            addTitle="Add Custom Domain"
            columns={columns}
            data={domains}
            modalContent={AddCustomDomainDialog}
            getRowId={(domain) => domain.domain}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    const command = new AddCustomDomainToApplicationEnvironment();
                    command.applicationId = applicationId!;
                    command.environmentId = environmentId!;
                    command.domain = output!.domain;
                    command.certificate = output!.certificate;
                    await command.execute();
                }
            }} />
    );
};
