// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

import { GridColDef } from '@mui/x-data-grid';
import { ModalResult } from '@aksio/applications-mui';
import { AddEnvironmentDialog, AddEnvironmentDialogOutput } from './AddEnvironmentDialog';
import { EnvironmentsForApplication } from 'API/applications/environments/EnvironmentsForApplication';
import { ApplicationEnvironment } from 'API/applications/environments/ApplicationEnvironment';
import { CreateApplicationEnvironment } from 'API/applications/environments/CreateApplicationEnvironment';
import { Guid } from '@aksio/fundamentals';
import { ValueEditorFor } from 'Components';
import { useRouteParams } from '../RouteParams';

const columns: GridColDef[] = [
    { field: 'name', headerName: 'Name', width: 250 },
    { field: 'displayName', headerName: 'Display Name', width: 250 },
    { field: 'shortName', headerName: 'Short Display Name', width: 150 }
];

export const Environments = () => {
    const { applicationId } = useRouteParams();
    const [environments] = EnvironmentsForApplication.use({ applicationId: applicationId! });

    return (
        <ValueEditorFor<AddEnvironmentDialogOutput, ApplicationEnvironment>
            addTitle="Add Environment"
            columns={columns}
            data={environments.data}
            modalContent={AddEnvironmentDialog}
            getRowId={(environment) => environment.id.environmentId}
            modalClosed={async (result, output) => {
                if (result == ModalResult.success) {
                    const command = new CreateApplicationEnvironment();
                    command.applicationId = applicationId!;
                    command.environmentId = Guid.create().toString();
                    command.name = output!.name;
                    command.displayName = output!.displayName;
                    command.shortName = output!.shortName;
                    command.azureSubscriptionId = output!.azureSubscription;
                    command.cloudLocation = output!.cloudLocation;
                    await command.execute();
                }
            }} />
    );
};
