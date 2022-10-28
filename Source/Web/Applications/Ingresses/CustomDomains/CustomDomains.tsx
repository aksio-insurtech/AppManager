// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { ModalResult } from '@aksio/cratis-mui';
import { GridColDef } from '@mui/x-data-grid';
import { ValueEditorFor } from 'Components';
import { AddCustomDomainToIngress } from 'API/applications/environments/ingresses/AddCustomDomainToIngress';
import { CustomDomain } from 'API/applications/environments/ingresses/CustomDomain';
import { AddCustomDomainDialog, AddCustomDomainDialogOutput } from './AddCustomDomainDialog';
import { useRouteParams } from '../../RouteParams';
import { Ingress } from 'API/applications/environments/ingresses/Ingress';


export interface CustomDomainsProps {
    ingress: Ingress;
}

const columns: GridColDef[] = [
    { field: 'domain', headerName: 'Domain', width: 250 }
];

export const CustomDomains = (props: CustomDomainsProps) => {
    const { applicationId, environmentId } = useRouteParams();
    const domains = props.ingress.customDomains ?? [];

    return (
        <ValueEditorFor<AddCustomDomainDialogOutput, CustomDomain>
            addTitle="Add Custom Domain"
            columns={columns}
            data={domains}
            modalContent={AddCustomDomainDialog}
            getRowId={(domain) => domain.domain}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    const command = new AddCustomDomainToIngress();
                    command.applicationId = applicationId!;
                    command.environmentId = environmentId!;
                    command.domain = output!.domain;
                    command.certificateId = output!.certificateId;
                    await command.execute();
                }
            }} />
    );
};
