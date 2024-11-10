import { TestBed } from '@angular/core/testing';

import { AuthService } from './auth-service.service';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { ChangePasswordModel } from '../models/changepassword.model';
import { ApiResponse } from '../models/ApiResponse{T}.model';

describe('AuthServiceService', () => {
  let service: AuthService;
  let httpMock: HttpTestingController;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports:[HttpClientTestingModule],
      providers : [AuthService]
    });
    service = TestBed.inject(AuthService);
    httpMock = TestBed.inject(HttpTestingController);
  });

  afterEach(() => {
    httpMock.verify();
  });
  

  it('should be created', () => {
    expect(service).toBeTruthy();
  });

  //----------change password
  
  it('should change password successfully', () => {
    // Arrange
    const mockChangePassword: ChangePasswordModel = {
      loginId: 'johndoe',
      oldPassword: 'oldPassword123',
      newPassword: 'newPassword123',
      newConfirmPassword: 'newPassword123'
    };

    const mockResponse: ApiResponse<string> = {
      success: true,
      message: 'Password changed successfully',
      data: ''
    };

    // Act
    service.changePassword(mockChangePassword).subscribe(response => {
      // Assert
      expect(response).toEqual(mockResponse);
      expect(response.message).toBe('Password changed successfully');
    });

    // Mock HTTP request
    const req = httpMock.expectOne('http://localhost:5038/api/Auth/ChangePassword');
    expect(req.request.method).toBe('PUT');
    req.flush(mockResponse);
  });

  it('should not change password successfully', () => {
    // Arrange
    const mockChangePassword: ChangePasswordModel = {
      loginId: 'johndoe',
      oldPassword: 'oldPassword123',
      newPassword: 'newPassword123',
      newConfirmPassword: 'neassword123'
    };

    const mockResponse: ApiResponse<string> = {
      success: false,
      message: 'Password not changed',
      data: ''
    };

    // Act
    service.changePassword(mockChangePassword).subscribe(response => {
      // Assert
      expect(response).toEqual(mockResponse);
      expect(response.message).toBe('Password not changed');
    });

    // Mock HTTP request
    const req = httpMock.expectOne('http://localhost:5038/api/Auth/ChangePassword');
    expect(req.request.method).toBe('PUT');
    req.flush(mockResponse);
  });

  it('should handle HTTP error while changing password', () => {
    // Arrange
    const mockChangePassword: ChangePasswordModel = {
      loginId: 'johndoe',
      oldPassword: 'oldPassword123',
      newPassword: 'newPassword123',
      newConfirmPassword: 'newPassword123'
    };

    const mockHttpError = {
      status: 500,
      statusText: 'Internal Server Error'
    };

    // Act & Assert
    service.changePassword(mockChangePassword).subscribe({
      next: () => fail('should have failed with the 500 error'),
      error: (error => {
        expect(error.status).toEqual(500);
        expect(error.statusText).toEqual('Internal Server Error');
      })
    });

    // Mock HTTP request
    const req = httpMock.expectOne('http://localhost:5038/api/Auth/ChangePassword');
    expect(req.request.method).toBe('PUT');
    req.flush({}, mockHttpError);
  });

});
