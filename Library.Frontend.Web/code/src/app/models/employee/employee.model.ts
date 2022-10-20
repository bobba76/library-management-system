import { Entity } from '@shared/models/entity.interface';

class EmployeeModel implements Entity {
  id: number;
  firstName: string;
  lastName: string;
  salary: number;
  role: EmployeeRole;
  managerId?: number;

  constructor() {
    this.id = 0;
    this.firstName = '';
    this.lastName = '';
    this.salary = null;
    this.role = EmployeeRole.Employee;
    this.managerId = 0;
  }
}

enum EmployeeRole {
  Employee = 1,
  Manager = 2,
  Ceo = 3,
}

// [GET NAME] => employeeRoleName.get(EmployeeRole.Employee)
const employeeRoleName = new Map<EmployeeRole, string>([
  [EmployeeRole.Employee, 'Anställd'],
  [EmployeeRole.Manager, 'Manager'],
  [EmployeeRole.Ceo, 'CEO'],
]);

// [GET SalaryCoefficient] => salaryCoefficient.get(EmployeeRole.Employee)
const salaryCoefficient = new Map<EmployeeRole, number>([
  [EmployeeRole.Employee, 1.125],
  [EmployeeRole.Manager, 1.725],
  [EmployeeRole.Ceo, 2.725],
]);

type CreateEmployeeInputModel = {
  firstName: string;
  lastName: string;
  salary: number;
  role: EmployeeRole;
  managerId?: number;
};

type UpdateEmployeeInputModel = {
  firstName?: string;
  lastName?: string;
  salary?: number;
  role?: EmployeeRole;
  managerId?: number;
};

export {
  EmployeeModel,
  EmployeeRole,
  employeeRoleName,
  salaryCoefficient,
  CreateEmployeeInputModel,
  UpdateEmployeeInputModel,
};
