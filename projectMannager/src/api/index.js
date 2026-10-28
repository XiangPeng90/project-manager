import request from '@/utils/request'

export function login(data) {
  return request({ url: '/api/auth/login', method: 'post', data })
}

export function getProjectList(params) {
  return request({ url: '/api/projects', method: 'get', params })
}

export function getProjectDetail(id) {
  return request({ url: `/api/projects/${id}`, method: 'get' })
}

export function createProject(data) {
  return request({ url: '/api/projects', method: 'post', data })
}

export function updateProject(id, data) {
  return request({ url: `/api/projects/${id}`, method: 'put', data })
}

export function disableProject(id) {
  return request({ url: `/api/projects/${id}/disable`, method: 'patch' })
}

export function getWorkPackagesByProject(projectId, params) {
  return request({ url: '/api/workpackages', method: 'get', params: { projectId, ...params } })
}

export function getWorkPackageDetail(id) {
  return request({ url: `/api/workpackages/${id}`, method: 'get' })
}

export function createWorkPackage(data) {
  return request({ url: '/api/workpackages', method: 'post', data })
}

export function updateWorkPackage(id, data) {
  return request({ url: `/api/workpackages/${id}`, method: 'put', data })
}

export function disableWorkPackage(id) {
  return request({ url: `/api/workpackages/${id}/disable`, method: 'patch' })
}

export function getProjectLedgers(projectId, params) {
  return request({ url: `/api/projects/${projectId}/ledgers`, method: 'get', params })
}

export function createProjectLedger(projectId, data) {
  return request({ url: `/api/projects/${projectId}/ledgers`, method: 'post', data })
}

export function updateProjectLedger(projectId, id, data) {
  return request({ url: `/api/projects/${projectId}/ledgers/${id}`, method: 'put', data })
}

export function voidProjectLedger(projectId, id) {
  return request({ url: `/api/projects/${projectId}/ledgers/${id}/void`, method: 'patch' })
}

export function getWorkPackageLedgers(wpId, params) {
  return request({ url: `/api/workpackages/${wpId}/ledgers`, method: 'get', params })
}

export function createWorkPackageLedger(wpId, data) {
  return request({ url: `/api/workpackages/${wpId}/ledgers`, method: 'post', data })
}

export function updateWorkPackageLedger(wpId, id, data) {
  return request({ url: `/api/workpackages/${wpId}/ledgers/${id}`, method: 'put', data })
}

export function voidWorkPackageLedger(wpId, id) {
  return request({ url: `/api/workpackages/${wpId}/ledgers/${id}/void`, method: 'patch' })
}

export function submitWorkPackage(wpId, data) {
  return request({ url: `/api/workpackages/${wpId}/submit`, method: 'post', data })
}
