import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule } from '@angular/common/http/testing';
import { SigninComponent } from './signin.component';
import { AuthService } from 'src/app/services/auth-service.service';
import { Router } from '@angular/router';
import { RouterTestingModule } from '@angular/router/testing';
import { FormsModule } from '@angular/forms';
import { of, throwError } from 'rxjs';
import { ApiResponse } from 'src/app/models/ApiResponse{T}.model';
import { AdminService } from 'src/app/services/admin.service';
import { User } from 'src/app/models/user.model';
import { Trainer } from 'src/app/models/trainer.model';

describe('SigninComponent', () => {
  let component: SigninComponent;
  let fixture: ComponentFixture<SigninComponent>;
  let authServiceSpy: jasmine.SpyObj<AuthService>;
  let adminServiceSpy: jasmine.SpyObj<AdminService>;
  let routerSpy: Router;

  beforeEach(() => {
    let authServiceSpyObj = jasmine.createSpyObj('AuthService', ['signIn'])
    let adminServiceSpyObj = jasmine.createSpyObj('AdminService', ['getTrainerByLoginId'])
    let routerSpyObj = jasmine.createSpyObj('Router', ['navigate']);
    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, FormsModule, RouterTestingModule],
      declarations: [SigninComponent],
      providers: [
        {
          provide: AuthService, useValue: authServiceSpyObj
        },
        {
          provide : AdminService, useValue : adminServiceSpyObj
        }
      ]
    });
    fixture = TestBed.createComponent(SigninComponent);
    authServiceSpy = TestBed.inject(AuthService) as jasmine.SpyObj<AuthService>;
    adminServiceSpy = TestBed.inject(AdminService) as jasmine.SpyObj<AdminService>;
    routerSpy = TestBed.inject(Router);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });

  it('should navigate to home when user signs in successfully (not 1st time)', () => {
    // Arrange
    component.username = "test";
    component.password = "fakePassword"
    const mockResponse: ApiResponse<string> = {
      data: 'fakeToken',
      success: true,
      message: ''
    }

    const mockUserData : ApiResponse<Trainer> = {
      data : {
        userId : 1,
        lastName : "Test",
        firstName : "Test",
        loginbit : true,
        loginId :"Test",
        job : {
          jobId : 1,
          jobName : "Test"
        },
        jobId : 1,
        role : 1,
        email : "S@gmail.com"
      },
      success : true,
      message : ''
      
    }
    spyOn(routerSpy, 'navigate')
    authServiceSpy.signIn.and.returnValue(of(mockResponse))
    adminServiceSpy.getTrainerByLoginId.and.returnValue(of(mockUserData));

    // Act
    component.login()

    // Assert
    expect(authServiceSpy.signIn).toHaveBeenCalledOnceWith(component.username, component.password)
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/home'])
  });
  it('should navigate to change password when user signs in successfully (1st time)', () => {
    // Arrange
    component.username = "test";
    component.password = "fakePassword"
    const mockResponse: ApiResponse<string> = {
      data: 'fakeToken',
      success: true,
      message: ''
    }
    

    const mockUserData : ApiResponse<Trainer> = {
      data : {
        userId : 1,
        lastName : "Test",
        firstName : "Test",
        loginbit : false,
        loginId :"Test",
        job : {
          jobId : 1,
          jobName : "Test"
        },
        jobId : 1,
        role : 1,
        email : "S@gmail.com"
      },
      success : true,
      message : ''
      
    }
    spyOn(routerSpy, 'navigate')
    authServiceSpy.signIn.and.returnValue(of(mockResponse))
    adminServiceSpy.getTrainerByLoginId.and.returnValue(of(mockUserData));

    // Act
    component.login()

    // Assert
    expect(authServiceSpy.signIn).toHaveBeenCalledOnceWith(component.username, component.password)
    expect(routerSpy.navigate).toHaveBeenCalledWith(['/changePassword'])
  });

  it('should set alert message when response is false', () => {
    // Arrange
    component.username = "test";
    component.password = "fakePassword"
    const mockResponse: ApiResponse<string> = {
      data: '',
      success: false,
      message: 'Verification failed'
    }
    const mockUserData : ApiResponse<Trainer> = {
      data : {
        userId : 1,
        lastName : "Test",
        firstName : "Test",
        loginbit : true,
        loginId :"Test",
        job : {
          jobId : 1,
          jobName : "Test"
        },
        jobId : 1,
        role : 1,
        email : "S@gmail.com"
      },
      success : true,
      message : ''
      
    }
    spyOn(window, 'alert')
    authServiceSpy.signIn.and.returnValue(of(mockResponse));
    adminServiceSpy.getTrainerByLoginId.and.returnValue(of(mockUserData));

    // Act
    component.login()

    // Assert
    expect(authServiceSpy.signIn).toHaveBeenCalledOnceWith(component.username, component.password)
    expect(window.alert).toHaveBeenCalledWith(mockResponse.message)
  });

  it('should set alert message when api returns error', () => {
    // Arrange
    component.username = "test";
    component.password = "fakePassword"
    const mockError = { error: { message: 'HTTP Error' } }

    const mockUserData : ApiResponse<Trainer> = {
      data : {
        userId : 1,
        lastName : "Test",
        firstName : "Test",
        loginbit : true,
        loginId :"Test",
        job : {
          jobId : 1,
          jobName : "Test"
        },
        jobId : 1,
        role : 1,
        email : "S@gmail.com"
      },
      success : true,
      message : ''
      
    }


    spyOn(window, 'alert')
    authServiceSpy.signIn.and.returnValue(throwError(mockError));
    adminServiceSpy.getTrainerByLoginId.and.returnValue(of(mockUserData));

    // Act
    component.login()

    // Assert
    expect(authServiceSpy.signIn).toHaveBeenCalledOnceWith(component.username, component.password)
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message)
  });

  it('should handle error when 1st time check function gives error', () => {
    // Arrange
    component.username = "test";
    const mockResponse: ApiResponse<Trainer> = {
      data: {} as Trainer,
      success: false,
      message: 'error fetching data'
    }
    
    spyOn(window, 'alert')
    adminServiceSpy.getTrainerByLoginId.and.returnValue(of(mockResponse));
    

    // Act
    component.login()

    // Assert
    expect(adminServiceSpy.getTrainerByLoginId).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith("error fetching data");
    
  });

  it('should handle HTTP error when 1st time check function gives error', () => {
    // Arrange
    component.username = "test";
    const mockError = { error: { message: 'HTTP Error' } }
    
    spyOn(window, 'alert')
    adminServiceSpy.getTrainerByLoginId.and.returnValue(throwError(mockError));
    

    // Act
    component.login()

    // Assert
    expect(adminServiceSpy.getTrainerByLoginId).toHaveBeenCalled();
    expect(window.alert).toHaveBeenCalledWith(mockError.error.message);
    
  }); 
});
 