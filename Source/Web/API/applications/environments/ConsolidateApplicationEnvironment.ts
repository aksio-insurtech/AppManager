/*---------------------------------------------------------------------------------------------
 *  **DO NOT EDIT** - This file is an automatically generated file.
 *--------------------------------------------------------------------------------------------*/

import { Command, CommandValidator, CommandPropertyValidators, useCommand, SetCommandValues, ClearCommandValues } from '@aksio/cratis-applications-frontend/commands';
import { Validator } from '@aksio/cratis-applications-frontend/validation';
import Handlebars from 'handlebars';

const routeTemplate = Handlebars.compile('/api/applications/{{applicationId}}/environments/{{environmentId}}/consolidate');

export interface IConsolidateApplicationEnvironment {
    applicationId?: string;
    environmentId?: string;
}

export class ConsolidateApplicationEnvironmentValidator extends CommandValidator {
    readonly properties: CommandPropertyValidators = {
        applicationId: new Validator(),
        environmentId: new Validator(),
    };
}

export class ConsolidateApplicationEnvironment extends Command<IConsolidateApplicationEnvironment> implements IConsolidateApplicationEnvironment {
    readonly route: string = '/api/applications/{{applicationId}}/environments/{{environmentId}}/consolidate';
    readonly routeTemplate: Handlebars.TemplateDelegate = routeTemplate;
    readonly validation: CommandValidator = new ConsolidateApplicationEnvironmentValidator();

    private _applicationId!: string;
    private _environmentId!: string;

    get requestArguments(): string[] {
        return [
            'applicationId',
            'environmentId',
        ];
    }

    get properties(): string[] {
        return [
            'applicationId',
            'environmentId',
        ];
    }

    get applicationId(): string {
        return this._applicationId;
    }

    set applicationId(value: string) {
        this._applicationId = value;
        this.propertyChanged('applicationId');
    }
    get environmentId(): string {
        return this._environmentId;
    }

    set environmentId(value: string) {
        this._environmentId = value;
        this.propertyChanged('environmentId');
    }

    static use(initialValues?: IConsolidateApplicationEnvironment): [ConsolidateApplicationEnvironment, SetCommandValues<IConsolidateApplicationEnvironment>, ClearCommandValues] {
        return useCommand<ConsolidateApplicationEnvironment, IConsolidateApplicationEnvironment>(ConsolidateApplicationEnvironment, initialValues);
    }
}
