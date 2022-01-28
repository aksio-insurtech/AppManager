/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command } from '@aksio/cratis-applications-frontend/commands';

export class RemoveApplication extends Command {
    readonly route: string = '/api/applications/remove';

    applicationId!: string;
}
